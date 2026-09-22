using System.Collections.Generic;
using System.Linq;
using Platformer.Mechanics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Platformer.EditorTools
{
    /// <summary>
    /// Starter window for the token group manager exercise.
    /// Candidates may extend this component as needed.
    /// </summary>
    public class TokenGroupToolWindow : EditorWindow
    {
        private TokenGroup _selectedGroup;

        private VisualElement _groupListContainer;
        private VisualElement _selectedGroupPanel;

        [MenuItem("Tools/Token Group Manager")]
        public static void OpenWindow()
        {
            GetWindow<TokenGroupToolWindow>("Token Groups");
        }

        private void OnSelectionChange()
        {
            RefreshAll();
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.paddingTop = root.style.paddingBottom =
                root.style.paddingLeft = root.style.paddingRight = 6;

            root.Add(new Label("Token Group Manager")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 4 }
            });

            root.Add(new HelpBox("Manage groups of tokens for designers. Complete the required TODOs.", HelpBoxMessageType.Info));

            var toolbar = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 4 } };
            toolbar.Add(new Button(RefreshAll) { text = "Refresh" });
            toolbar.Add(new Button(CreateGroup) { text = "Create Group" });
            root.Add(toolbar);

            root.Add(new Label("Groups")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 6 }
            });

            var scroll = new ScrollView { style = { minHeight = 160 } };
            _groupListContainer = new VisualElement();
            scroll.Add(_groupListContainer);
            root.Add(scroll);

            root.Add(new Label("Selected Group")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 6 }
            });

            _selectedGroupPanel = new VisualElement();
            root.Add(_selectedGroupPanel);

            RefreshAll();
        }

        private void RefreshAll()
        {
            RebuildGroupList();
            RebuildSelectedGroupPanel();
        }

        private void RebuildGroupList()
        {
            if (_groupListContainer == null) return;

            _groupListContainer.Clear();
            var groups = GetGroupsInScene();

            if (groups.Count == 0)
            {
                _groupListContainer.Add(new HelpBox("No token groups found in the scene.", HelpBoxMessageType.Warning));
                return;
            }

            foreach (var group in groups)
            {
                var groupBox = new VisualElement();
                groupBox.style.borderTopWidth = groupBox.style.borderBottomWidth =
                    groupBox.style.borderLeftWidth = groupBox.style.borderRightWidth = 1;
                groupBox.style.borderTopColor = groupBox.style.borderBottomColor =
                    groupBox.style.borderLeftColor = groupBox.style.borderRightColor = new Color(0.3f, 0.3f, 0.3f);
                groupBox.style.paddingTop = groupBox.style.paddingBottom =
                    groupBox.style.paddingLeft = groupBox.style.paddingRight = 4;
                groupBox.style.marginBottom = 3;

                var headerRow = new VisualElement { style = { flexDirection = FlexDirection.Row } };

                var capturedGroup = group;
                var nameButton = new Button(() =>
                {
                    _selectedGroup = capturedGroup;
                    Selection.activeGameObject = capturedGroup.gameObject;
                    RebuildSelectedGroupPanel();
                }) { text = group.name, style = { flexGrow = 1 } };

                headerRow.Add(nameButton);
                headerRow.Add(new Label($"{group.tokens.Count} token(s)") { style = { width = 80, unityTextAlign = TextAnchor.MiddleRight } });
                groupBox.Add(headerRow);

                var buttonRow = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 2 } };
                buttonRow.Add(new Button(() => { AddSelectedTokens(capturedGroup); RefreshAll(); }) { text = "Add Selected" });
                buttonRow.Add(new Button(() => { RemoveSelectedTokens(capturedGroup); RefreshAll(); }) { text = "Remove Selected" });
                buttonRow.Add(new Button(() => FrameGroup(capturedGroup)) { text = "Frame" });
                groupBox.Add(buttonRow);

                _groupListContainer.Add(groupBox);
            }
        }

        private void RebuildSelectedGroupPanel()
        {
            if (_selectedGroupPanel == null) return;

            _selectedGroupPanel.Clear();

            if (_selectedGroup == null)
            {
                _selectedGroupPanel.Add(new HelpBox("Select or create a token group.", HelpBoxMessageType.None));
                return;
            }

            var box = new VisualElement();
            box.style.borderTopWidth = box.style.borderBottomWidth =
                box.style.borderLeftWidth = box.style.borderRightWidth = 1;
            box.style.borderTopColor = box.style.borderBottomColor =
                box.style.borderLeftColor = box.style.borderRightColor = new Color(0.3f, 0.3f, 0.3f);
            box.style.paddingTop = box.style.paddingBottom =
                box.style.paddingLeft = box.style.paddingRight = 4;

            var nameField = new TextField("Name") { value = _selectedGroup.name };
            nameField.RegisterValueChangedCallback(evt => _selectedGroup.name = evt.newValue);
            box.Add(nameField);

            var posField = new Vector3Field("Group Position") { value = _selectedGroup.transform.position };
            posField.RegisterValueChangedCallback(evt => MoveGroup(_selectedGroup, evt.newValue));
            box.Add(posField);

            var actionRow = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 2 } };
            actionRow.Add(new Button(() => { AddSelectedTokens(_selectedGroup); RefreshAll(); }) { text = "Add Selected" });
            actionRow.Add(new Button(() => { RemoveSelectedTokens(_selectedGroup); RefreshAll(); }) { text = "Remove Selected" });
            actionRow.Add(new Button(() => FrameGroup(_selectedGroup)) { text = "Frame In Scene" });
            box.Add(actionRow);

            box.Add(new Label("Tokens") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 4 } });

            if (_selectedGroup.tokens == null || _selectedGroup.tokens.Count == 0)
            {
                box.Add(new Label("No tokens assigned."));
            }
            else
            {
                foreach (var token in _selectedGroup.tokens.ToList())
                {
                    var tokenRow = new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 2 } };

                    var objectField = new ObjectField
                    {
                        objectType = typeof(TokenInstance),
                        value = token,
                        allowSceneObjects = true,
                        style = { flexGrow = 1 }
                    };
                    tokenRow.Add(objectField);
                    box.Add(tokenRow);
                }
            }

            _selectedGroupPanel.Add(box);
        }

        private List<TokenGroup> GetGroupsInScene()
        {
            return FindObjectsOfType<TokenGroup>().OrderBy(group => group.name).ToList();
        }

        private List<TokenInstance> GetSelectedTokens()
        {
            return Selection.gameObjects
                .Select(go => go.GetComponent<TokenInstance>())
                .Where(token => token != null)
                .Distinct()
                .ToList();
        }

        private void CreateGroup()
        {
            Debug.Log("TODO: CreateGroup");
        }

        private void AddSelectedTokens(TokenGroup group)
        {
            Debug.Log("TODO: AddSelectedTokens");
        }

        private void RemoveSelectedTokens(TokenGroup group)
        {
            Debug.Log("TODO: RemoveSelectedTokens");
        }

        private void MoveGroup(TokenGroup group, Vector3 newPosition)
        {
            Debug.Log("TODO: MoveGroup");
        }

        private void FrameGroup(TokenGroup group)
        {
            Debug.Log("TODO: FrameGroup");
        }
    }
}
