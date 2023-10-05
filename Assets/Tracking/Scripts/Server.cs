using System.Collections;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/* Currently very messy because both the server code and hand-drawn code is all in the same file here.
 * But it is still fairly straightforward to use as a reference/base.
 */

namespace Tracking
{
    [DefaultExecutionOrder(-1)]
    public class Server :
        MonoBehaviour
    {
        public Transform bodyParent;
        public GameObject landmarkPrefab;
        public GameObject linePrefab;
        public GameObject headPrefab;
        public bool enableHead = false;
        public float multiplier = 10f;
        public float landmarkScale = 1f;
        public float maxSpeed = 50f;
        public float debug_samplespersecond;
        public int samplesForPose = 1;
        public bool active;

// private NamedPipeServerStream server;
        private Body body;

// these virtual transforms are not actually provided by mediapipe pose, but are required for avatars.
// so I just manually compute them
        private Transform virtualNeck;
        private Transform virtualHip;

        public Transform GetLandmark(LandmarkIndex mark)
        {
            return body.instances[(int)mark].transform;
        }

        public Transform GetVirtualNeck()
        {
            return virtualNeck;
        }

        public Transform GetVirtualHip()
        {
            return virtualHip;
        }

        private void Start()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            body = new Body(bodyParent, landmarkPrefab, linePrefab, landmarkScale, enableHead ? headPrefab : null);
            virtualNeck = new GameObject("VirtualNeck").transform;
            virtualHip = new GameObject("VirtualHip").transform;

            Thread t = new Thread(new ThreadStart(Run));
            t.Start();
        }

        private void Update()
        {
            UpdateBody(body);
        }

        private void UpdateBody(Body b)
        {
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {
                if (b.positionsBuffer[i].accumulatedValuesCount < samplesForPose)
                    continue;

                b.localPositionTargets[i] = b.positionsBuffer[i].value /
                    (float)b.positionsBuffer[i].accumulatedValuesCount * multiplier;
                b.positionsBuffer[i] = new AccumulatedBuffer(Vector3.zero, 0);
            }

            Vector3 offset = Vector3.zero;
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {
                Vector3 p = b.localPositionTargets[i] - offset;
                b.instances[i].transform.localPosition =
                    Vector3.MoveTowards(b.instances[i].transform.localPosition, p, Time.deltaTime * maxSpeed);
            }

            virtualNeck.transform.position = (b.instances[(int)LandmarkIndex.RIGHT_SHOULDER].transform.position +
                                              b.instances[(int)LandmarkIndex.LEFT_SHOULDER].transform.position) / 2f;
            virtualHip.transform.position = (b.instances[(int)LandmarkIndex.RIGHT_HIP].transform.position +
                                             b.instances[(int)LandmarkIndex.LEFT_HIP].transform.position) / 2f;

            b.UpdateLines();
        }

        public void SetVisible(bool visible)
        {
            bodyParent.gameObject.SetActive(visible);
        }

        private void Run()
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            // Open the named pipe.
            // server = new NamedPipeServerStream("UnityMediaPipeBody", PipeDirection.InOut, 99, PipeTransmissionMode.Message);
            var receiver = new Receiver();
            receiver.StartReceiver(CancellationToken.None).Forget();

            print("Waiting for connection...");
            // server.WaitForConnection();
            while (receiver.ReceivedMessage == null)
            {
                Thread.Sleep(100);
                continue;
            }

            print("Connected.");
            // var br = new BinaryReader(server, Encoding.UTF8);

            while (true)
            {
                try
                {
                    Body h = body;
                    // var len = (int)br.ReadUInt32();
                    // var str = new string(br.ReadChars(len));
                    //
                    // string[] lines = str.Split('\n');
                    // foreach (string l in lines)
                    // {
                    //     if (string.IsNullOrWhiteSpace(l))
                    //         continue;
                    //     string[] s = l.Split('|');
                    //     if (s.Length < 4) continue;
                    //     int i;
                    //     if (!int.TryParse(s[0], out i)) continue;
                    //     h.positionsBuffer[i].value += new Vector3(float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));
                    //     h.positionsBuffer[i].accumulatedValuesCount += 1;
                    //     h.active = true;
                    // }

                    var landmarks =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<LandmarkJson[]>(receiver.ReceivedMessage);
                    for (int i = 0; i < landmarks.Length; ++i)
                    {
                        h.positionsBuffer[i].value += new Vector3(float.Parse(landmarks[i].X),
                            -float.Parse(landmarks[i].Z),
                            -float.Parse(landmarks[i].Y));
                        h.positionsBuffer[i].accumulatedValuesCount += 1;
                        h.active = true;
                    }
                }
                catch (EndOfStreamException)
                {
                    print("Client Disconnected");
                    break;
                }
            }
        }

        private void OnDisable()
        {
            print("Client disconnected.");
            // server.Close();
            // server.Dispose();
        }

        const int LANDMARK_COUNT = 33;
        const int LINES_COUNT = 11;
    }
}