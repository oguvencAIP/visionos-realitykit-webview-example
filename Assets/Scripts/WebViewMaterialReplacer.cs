using UnityEngine;
using UnityEngine.Rendering;
using Vuplex.WebView;

class WebViewMaterialReplacer : MonoBehaviour {

    public BaseWebViewPrefab webViewPrefab;
    Texture2D _texture;
    bool _textureUpdateInProgress;

    async void Start() {

        await webViewPrefab.WaitUntilInitialized();
        var renderPipeline = GraphicsSettings.defaultRenderPipeline;
        var isUrp = renderPipeline != null && renderPipeline.GetType().Name == "UniversalRenderPipelineAsset";
        var shaderName = isUrp ? "Universal Render Pipeline/Unlit" : "UI/Default";
        var material = new Material(Shader.Find(shaderName));
        var size = webViewPrefab.WebView.Size;
        _texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
        material.mainTexture = _texture;
        webViewPrefab.Material = material;
    }

    async void Update() {

        if (_textureUpdateInProgress || _texture == null || webViewPrefab.WebView == null) {
            return;
        }
        _textureUpdateInProgress = true;
        var textureData = await webViewPrefab.WebView.GetRawTextureData();
        _texture.LoadRawTextureData(textureData);
        _texture.Apply();
        _textureUpdateInProgress = false;
    }
}