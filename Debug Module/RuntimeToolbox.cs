using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;                                          
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
using Microsoft.Xna.Framework;
using Color = System.Drawing.Color;
using Control = Blish_HUD.Controls.Control;
using Container = Blish_HUD.Controls.Container;

namespace Debug_Module {                                                                                
    public partial class RuntimeToolbox : Form {
        public RuntimeToolbox() {
            InitializeComponent();
        }

        #region Runtime Viewer

        private bool _pendingRefresh = false;

        private void BttnRefresh_Click(object sender, EventArgs e) {
            BuildBlishHudTree(treeControlsAndEntities.Nodes);

            PurgeEmpty(treeControlsAndEntities.Nodes);
        }

        private void PurgeEmpty(TreeNodeCollection parentsNodes) {
            var nodes = new TreeNode[parentsNodes.Count];
            parentsNodes.CopyTo(nodes, 0);

            foreach (var node in nodes) {
                if (node.Tag == null && node.Parent.Parent != null) {
                    node.Remove();
                } else {
                    PurgeEmpty(node.Nodes);
                }
            }
        }

        private TreeNode GetOrAddTreeNodeFromName(TreeNodeCollection parentsNodes, string nodeName) {
            foreach (TreeNode node in parentsNodes) {
                if (node.Text == nodeName) {
                    return node;
                }
            }

            var newNode = parentsNodes.Add(nodeName);
            newNode.NodeFont = new Font(treeControlsAndEntities.Font, FontStyle.Bold | FontStyle.Italic);
            return newNode;
        }

        private void BuildBlishHudTree(TreeNodeCollection parentsNodes) {
            if (typeof(GraphicsDeviceManager).GetField("_game", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(GameService.Graphics.GraphicsDeviceManager) is Game game) {
                var blishHudNode = GetOrAddTreeNodeFromObj(parentsNodes, game);
                blishHudNode.NodeFont = new Font(treeControlsAndEntities.Font, FontStyle.Bold);

                BuildEntityTree(blishHudNode.Nodes);
                BuildControlTree(blishHudNode.Nodes);
                BuildGameServiceTree(blishHudNode.Nodes);
                BuildModuleTree(blishHudNode.Nodes);
            }
        }

        private void BuildEntityTree(TreeNodeCollection parentsNodes) {
            var worldRoot = GetOrAddTreeNodeFromObj(parentsNodes, GameService.Graphics.World);

            foreach (var entity in GameService.Graphics.World.Entities) {
                GetOrAddTreeNodeFromObj(worldRoot.Nodes, entity);
            }
        }

        private void BuildControlTree(TreeNodeCollection parentsNodes, Control control = null) {
            control = control ?? GameService.Graphics.SpriteScreen;

            var controlNode = GetOrAddTreeNodeFromObj(parentsNodes, control);

            if (control is Container container) {
                foreach (var childControl in container) {
                    BuildControlTree(controlNode.Nodes, childControl);
                }

                container.ChildAdded   += ContainerOnChildAdded;
                container.ChildRemoved += ContainerOnChildRemoved;
            }
        }

        private void BuildGameServiceTree(TreeNodeCollection parentsNodes) {
            TreeNode gameServiceRoot = GetOrAddTreeNodeFromName(parentsNodes, "GameServices");

            foreach (var gameService in GameService.All) {
                GetOrAddTreeNodeFromObj(gameServiceRoot.Nodes, gameService);
            }
        }

        private void BuildModuleTree(TreeNodeCollection parentsNodes) {
            TreeNode modulesRoot = GetOrAddTreeNodeFromName(parentsNodes, "Modules");

            foreach (var modules in GameService.Module.Modules) {
                GetOrAddTreeNodeFromObj(modulesRoot.Nodes, modules);
            }
        }

        private TreeNode GetOrAddTreeNodeFromObj(TreeNodeCollection parentsNodes, object obj) {
            foreach (TreeNode siblingNode in parentsNodes) {
                if (object.Equals(siblingNode.Tag, obj)) {
                    return siblingNode;
                }
            }

            string shortAssembly = obj.GetType().AssemblyQualifiedName.Split(',')[0];

            var newNode = new TreeNode($"{obj.GetType().Name} [{shortAssembly}]") {
                Tag = obj
            };

            parentsNodes.Add(newNode);

            return newNode;
        }

        private void ContainerOnChildAdded(object sender, ChildChangedEventArgs e) {
            var parentsNodes = treeControlsAndEntities.Nodes.Descendants().FirstOrDefault(n => n.Tag == sender)?.Nodes;

            BuildControlTree(parentsNodes, e.ChangedChild);
        }

        private void ContainerOnChildRemoved(object sender, ChildChangedEventArgs e) {
            var removedChildNode = treeControlsAndEntities.Nodes.Descendants().FirstOrDefault(n => n.Tag == e.ChangedChild);

            removedChildNode?.Remove();

            if (e.ChangedChild is Container container) {
                container.ChildAdded   -= ContainerOnChildAdded;
                container.ChildRemoved -= ContainerOnChildRemoved;
            }
        }

        private void TreeControlsAndEntities_AfterSelect(object sender, TreeViewEventArgs e) {
            if (propGrid.SelectedObject is INotifyPropertyChanged prevObj) {
                prevObj.PropertyChanged -= NotifObjOnPropertyChanged;
            }

            var selectedNode = treeControlsAndEntities.SelectedNode;

            if (treeControlsAndEntities.SelectedNode == null) {
                propGrid.SelectedObject = null;
                return;
            }

            var selectedNodeObj = selectedNode.Tag;

            // TreeNode.Parent is null when it is one of the root TreeNodes
            if (selectedNodeObj == null && selectedNode.Parent.Parent != null) {
                propGrid.SelectedObject = null;
                selectedNode.Remove();
                return;
            }

            propGrid.SelectedObject = selectedNodeObj;

            _pendingRefresh = false;

            if (propGrid.SelectedObject is INotifyPropertyChanged notifObj) {
                notifObj.PropertyChanged += NotifObjOnPropertyChanged;
            }
        }

        private void NotifObjOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            _pendingRefresh = true;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e) {
            if (cbAutoRefresh.Checked && _pendingRefresh) {
                propGrid.Refresh();
                _pendingRefresh = false;
            }
        }

        private void BttnSelectControl_Click(object sender, MouseEventArgs e) {
            GameService.GameIntegration.FocusGw2();

            DebugModule.ModuleInstance.ActivateRuntimeViewerPicker();
        }

        internal void SetActiveControl(object obj, TreeNodeCollection parentsNodes = null) {
            parentsNodes = parentsNodes ?? treeControlsAndEntities.Nodes;

            var nodes = new TreeNode[parentsNodes.Count];
            parentsNodes.CopyTo(nodes, 0);

            foreach (var node in nodes) {
                if (node.Tag == obj) {
                    void ExpandUpwards(TreeNode parent) {
                        parent.Expand();

                        if (parent.Parent != null) {
                            ExpandUpwards(parent.Parent);
                        }
                    }

                    ExpandUpwards(node.Parent);
                    treeControlsAndEntities.SelectedNode = node;
                    return;
                }

                SetActiveControl(obj, node.Nodes);
            }
        }

        #endregion

        private void RuntimeToolbox_Shown(object sender, EventArgs e) {
            bttnRefresh.PerformClick();
        }
    }
}
