using System.Collections;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/* Currently very messy because both the server code and hand-drawn code is all in the same file here.
 * But it is still fairly straightforward to use as a reference/base.
 */

namespace Tracking
{
    public class Server :
        MonoBehaviour
    {
        public Transform bodyParent;
        public bool enableHead = false;
        public float landmarkScale = 1f;
        public bool active;

        private Body _body;


        // public Transform GetLandmark(LandmarkIndex mark)
        // {
        //     return body.instances[(int)mark].transform;
        // }


        private void Start()
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;


            var token = this.GetCancellationTokenOnDestroy();
            StartServer(token).Forget();
        }


        public void SetVisible(bool visible)
        {
            bodyParent.gameObject.SetActive(visible);
        }

        private async UniTask StartServer(CancellationToken token)
        {
            print("Waiting for connection...");

            while (!token.IsCancellationRequested)
            {
                var client = new UdpClient(5000);
                try
                {
                    var result = await client.ReceiveAsync();
                    var landmarks =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<LandmarkJson[]>(
                            Encoding.UTF8.GetString(result.Buffer));
                    for (int i = 0; i < landmarks.Length; ++i)
                    {
                        _body.positionsBuffer[i].value += new Vector3(float.Parse(landmarks[i].X),
                            -float.Parse(landmarks[i].Z),
                            -float.Parse(landmarks[i].Y));
                        _body.positionsBuffer[i].accumulatedValuesCount += 1;
                        _body.active = true;
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
        }
    }
}