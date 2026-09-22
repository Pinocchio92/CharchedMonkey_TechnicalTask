using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Platformer.EditorTools
{
    /// <summary>
    /// Starter window for the environment texture importer exercise.
    /// Candidates may extend this component as needed.
    /// </summary>
    public class EnvironmentTextureImporterToolWindow : EditorWindow
    {
        private static readonly string[] SupportedExtensions =
        {
            ".png",
            ".jpg",
            ".jpeg"
        };

        private string _sourceFolder = string.Empty;

        private TextField _environmentNameField;
        private Label _sourceFolderLabel;
        private IntegerField _pixelsPerUnitField;
        private Label _destinationLabel;
        private HelpBox _statusBox;

        [MenuItem("Tools/Environment Texture Importer")]
        public static void OpenWindow()
        {
            GetWindow<EnvironmentTextureImporterToolWindow>("Texture Importer");
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.paddingTop = root.style.paddingBottom =
                root.style.paddingLeft = root.style.paddingRight = 6;

            root.Add(new Label("Environment Texture Importer")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 4 }
            });

            root.Add(new HelpBox(
                "Import textures into Assets/Environment/<EnvironmentName>/Sprites and configure them as sprites.",
                HelpBoxMessageType.Info));

            _environmentNameField = new TextField("Environment Name");
            _environmentNameField.RegisterValueChangedCallback(_ => UpdateDestinationPreview());
            root.Add(_environmentNameField);

            var folderRow = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 2 } };
            folderRow.Add(new Label("Source Folder") { style = { width = 120, unityTextAlign = TextAnchor.MiddleLeft } });
            _sourceFolderLabel = new Label("<None>") { style = { flexGrow = 1, unityTextAlign = TextAnchor.MiddleLeft } };
            folderRow.Add(_sourceFolderLabel);
            folderRow.Add(new Button(BrowseForFolder) { text = "Browse", style = { width = 80 } });
            root.Add(folderRow);

            _pixelsPerUnitField = new IntegerField("Pixels Per Unit") { value = 100 };
            root.Add(_pixelsPerUnitField);

            _destinationLabel = new Label("Destination: Assets/Environment/<EnvironmentName>/Sprites")
            {
                style = { marginTop = 4, marginBottom = 4 }
            };
            root.Add(_destinationLabel);

            root.Add(new Button(ImportTextures) { text = "Import Textures", style = { marginTop = 4 } });

            _statusBox = new HelpBox(string.Empty, HelpBoxMessageType.None);
            _statusBox.style.display = DisplayStyle.None;
            root.Add(_statusBox);
        }

        private void UpdateDestinationPreview()
        {
            _destinationLabel.text = "Destination: " + GetDestinationFolderPreview();
        }

        private void BrowseForFolder()
        {
            var selectedPath = EditorUtility.OpenFolderPanel("Select Source Folder", Application.dataPath, string.Empty);

            if (!string.IsNullOrWhiteSpace(selectedPath))
            {
                _sourceFolder = selectedPath;
                _sourceFolderLabel.text = _sourceFolder;
            }
        }

        private string GetDestinationFolderPreview()
        {
            if (string.IsNullOrWhiteSpace(_environmentNameField?.value))
                return "Assets/Environment/<EnvironmentName>/Sprites";

            return $"Assets/Environment/{_environmentNameField.value}/Sprites";
        }

        private void ImportTextures()
        {
            SetStatus("TODO: Implement ImportTextures().");
        }

        private void SetStatus(string message)
        {
            _statusBox.text = message;
            _statusBox.style.display = string.IsNullOrEmpty(message) ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void EnsureFolderExists(string assetFolderPath)
        {
            Debug.Log($"TODO: EnsureFolderExists {assetFolderPath}");
        }

        private void ConfigureAsSprite(string assetPath)
        {
            Debug.Log($"TODO: ConfigureAsSprite {assetPath}");
        }

        private string[] GetSupportedFiles(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
                return new string[0];

            return Directory
                .GetFiles(sourceFolder)
                .Where(path => SupportedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                .ToArray();
        }
    }
}
