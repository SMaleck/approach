using System;
using System.Collections.Generic;

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Fluent API for building a behaviour tree.
    /// Modified, but originally based on https://github.com/ashleydavis/Fluent-Behaviour-Tree
    /// </summary>
    public class BehaviourTreeBuilder
    {
        private readonly Stack<IParentBehaviourTreeNode> parentNodeStack;
        private IBehaviourTreeNode curNode;
        private bool _isBuilt;

        public BehaviourTreeBuilder()
        {
            parentNodeStack = new Stack<IParentBehaviourTreeNode>();
        }

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder Do(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            AssertCanAddLeaf();

            var actionNode = new ActionNode(name, fn);
            parentNodeStack.Peek().AddChild(actionNode);
            return this;
        }

        /// <summary>
        /// Like an action node... but the function can return true/false and is mapped to success/failure.
        /// </summary>
        public BehaviourTreeBuilder Condition(string name, Func<TimeData, bool> fn)
        {
            return Do(name, t => fn(t) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure);
        }

        /// <summary>
        /// Create an inverter node that inverts the success/failure of its children.
        /// </summary>
        public BehaviourTreeBuilder Inverter(string name)
        {
            AssertCanModify();

            var inverterNode = new InverterNode(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(inverterNode);
            }

            parentNodeStack.Push(inverterNode);
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviourTreeBuilder Sequence(string name)
        {
            AssertCanModify();

            var sequenceNode = new SequenceNode(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(sequenceNode);
            }

            parentNodeStack.Push(sequenceNode);
            return this;
        }

        /// <summary>
        /// Create a parallel node.
        /// </summary>
        public BehaviourTreeBuilder Parallel(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            AssertCanModify();

            var parallelNode = new ParallelNode(name, numRequiredToFail, numRequiredToSucceed);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(parallelNode);
            }

            parentNodeStack.Push(parallelNode);
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviourTreeBuilder Selector(string name)
        {
            AssertCanModify();

            var selectorNode = new SelectorNode(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(selectorNode);
            }

            parentNodeStack.Push(selectorNode);
            return this;
        }

        /// <summary>
        /// Splice a sub tree into the parent tree.
        /// </summary>
        public BehaviourTreeBuilder Splice(IBehaviourTreeNode subTree)
        {
            AssertCanSplice(subTree);

            parentNodeStack.Peek().AddChild(subTree);
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public IBehaviourTreeNode Build()
        {
            AssertCanBuild();

            _isBuilt = true;
            return curNode;
        }

        /// <summary>
        /// Ends a sequence of children.
        /// </summary>
        public BehaviourTreeBuilder End()
        {
            curNode = parentNodeStack.Pop();
            return this;
        }

        private void AssertCanModify()
        {
            if (_isBuilt)
            {
                throw new ApplicationException("Cannot modify or builkd already built tree");
            }
        }

        private void AssertCanAddLeaf()
        {
            AssertCanModify();

            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't create an unnested ActionNode, it must be a leaf node.");
            }

        }

        private void AssertCanSplice(IBehaviourTreeNode subTree)
        {
            AssertCanModify();

            if (subTree == null)
            {
                throw new ArgumentNullException("subTree");
            }

            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't splice an unnested sub-tree, there must be a parent-tree.");
            }
        }

        private void AssertCanBuild()
        {
            AssertCanModify();

            if (curNode == null)
            {
                throw new ApplicationException("Can't create a behaviour tree with zero nodes");
            }
        }
    }
}