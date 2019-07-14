using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;                                          
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Entities;
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
            treeControlsAndEntities.Nodes.Clear();

            var worldRoot = treeControlsAndEntities.Nodes.Add("World [Entities]");

            foreach (var entity in GameService.Graphics.World.Entities) {
                var entityNode = GetTreeNodeFromObj(entity);

                worldRoot.Nodes.Add(entityNode);
            }

            BuildControlTree(GameService.Graphics.SpriteScreen, treeControlsAndEntities.Nodes);

            BuildGameServiceTree(treeControlsAndEntities.Nodes);

            BuildModuleTree(treeControlsAndEntities.Nodes);
        }

        private void BuildEntityTree(Entity entity, TreeNodeCollection parentsNodes) {
            // TODO: Must support EntityContainer<T> before we can really use this

            var entityNode = GetTreeNodeFromObj(entity);

            parentsNodes.Add(entityNode);
        }

        private void BuildControlTree(Control control, TreeNodeCollection parentsNodes) {
            var controlNode = GetTreeNodeFromObj(control);

            if (control is Container container) {
                foreach (var childControl in container) {
                    BuildControlTree(childControl, controlNode.Nodes);
                }

                //container.ChildAdded   += ContainerOnChildAdded;
                //container.ChildRemoved += ContainerOnChildRemoved;
            }

            parentsNodes.Add(controlNode);
        }

        private void BuildGameServiceTree(TreeNodeCollection parentsNodes) {
            var gameServiceRoot = parentsNodes.Add("GameServices");

            foreach (var gameService in GameService.All) {
                var gameServiceNode = GetTreeNodeFromObj(gameService);

                gameServiceRoot.Nodes.Add(gameServiceNode);
            }
        }

        private void BuildModuleTree(TreeNodeCollection parentsNodes) {
            var modulesRoot = parentsNodes.Add("Modules");

            foreach (var modules in GameService.Module.Modules) {
                var moduleNode = GetTreeNodeFromObj(modules);

                modulesRoot.Nodes.Add(moduleNode);
            }
        }

        private void ContainerOnChildAdded(object sender, ChildChangedEventArgs e) {
            var parentsNodes = treeControlsAndEntities.Nodes.Descendants().FirstOrDefault(n => n.Tag == sender)?.Nodes;

            BuildControlTree(e.ChangedChild, parentsNodes);
        }

        private void ContainerOnChildRemoved(object sender, ChildChangedEventArgs e) {
            var removedChildNode = treeControlsAndEntities.Nodes.Descendants().FirstOrDefault(n => n.Tag == e.ChangedChild);

            removedChildNode?.Remove();

            if (e.ChangedChild is Container container) {
                container.ChildAdded -= ContainerOnChildAdded;
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
            if (selectedNodeObj == null && selectedNode.Parent != null) {
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

        private TreeNode GetTreeNodeFromObj(object obj) {
            string shortAssembly = obj.GetType().AssemblyQualifiedName.Split(',')[0];

            return new TreeNode($"{obj.GetType().Name} [{shortAssembly}]") {
                Tag = obj
            };
        }

        #endregion
    }
}
