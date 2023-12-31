# AvoidGame

2023 農学部学祭で制作したゲーム

## 議事録

<https://tuatmcc.kibe.la/notes/3642>

<https://tuatmcc.kibe.la/notes/3674>

## 環境

- Windows11 Home
- UnityEdior 2022.3.10f1
- Universal Render Pipeline

## 関連リンク

### [tuatmcc/MediaPipeSocket](https://github.com/tuatmcc/MediaPipeSocket)

モーションキャプチャ用の Python リポジトリ

### [tuatmcc/AvoidGameAssets](https://github.com/tuatmcc/AvoidGameAssets)

Blender で作ったアセットたち

### [tuatmcc/UdpConnectingSystem](https://github.com/tuatmcc/UdpConnectingSystem)

Python, C#間で LAN ケーブルつないでソケット通信をするやつ

## 概要

Webカメラによるモーションキャプチャでゲーム内キャラクターを動かしてアイテムを取ったり避けたりするゲーム。

あらかじめ[tuatmcc/MediaPipeSocket](https://github.com/tuatmcc/MediaPipeSicket)が起動している必要がある。

本体部分は大体`Assets/AvoidGame`にある(一部プレハブやモデル、マテリアルは`Assets/DesignDemo`にある)

### シーン構成

1. Title - タイトル画面 `SecneTransitionManager` がある。
2. Description - ゲーム説明
3. Calibration - キャブレーション & モーショントラッキング動作確認
4. Play - 自分の体を動かしてアイテムを取り、障害物をよける
5. Result - リザルト画面
6. Tester - テスト用のシーン。このシーンから始めるとテスト用の `TestSceneTransitionManager` がある。

### 使用ツール

- `Universal Render Pipeline`
- `Cinemachine`: カメラワーク
- `Zenject(Extenject)`:
- `extOSC`:
- `Animation Rigging`
- `InputSystems`

### 外部アセット

- フォント
    - [Noto Sans JP](https://fonts.google.com/noto/specimen/Noto+Sans+JP)
    - 
- コース・周辺のビル
    - [⑨な3Dショップ サイバーシティ2050 - BOOTH](https://booth.pm/ja/items/4916802)
- 人型アニメーション
    - [Mixamo](https://mixamo.com)
    - [ねここや EmoteBox - BOOTH](https://booth.pm/ja/items/2291899)
