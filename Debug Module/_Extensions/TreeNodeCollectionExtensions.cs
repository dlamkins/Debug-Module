using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Debug_Module {
    public static class TreeNodeCollectionExtensions {

        internal static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c) {
            foreach (var node in c.OfType<TreeNode>()) {
                yield return node;

                foreach (var child in node.Nodes.Descendants()) {
                    yield return child;
                }
            }
        }

    }
}
