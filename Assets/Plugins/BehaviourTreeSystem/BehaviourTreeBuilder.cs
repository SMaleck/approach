using BehaviourTreeSystem.DecoratorNodes;
using BehaviourTreeSystem.LeafNodes;
using BehaviourTreeSystem.StructuralNodes;
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
        private readonly BehaviourTreeIdGenerator _idGenerator;
        private readonly Stack<IStructuralBehaviourTreeNode> _parentNodeStack;

        private IBehaviourTreeNode _curNode;
        private bool _isBuilt;

        public BehaviourTreeBuilder()
        {
            _idGenerator = new BehaviourTreeIdGenerator();
            _parentNodeStack = new Stack<IStructuralBehaviourTreeNode>();
        }

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder Do(Func<TimeData, BehaviourTreeStatus> fn)
        {
            AssertCanAddLeaf();
            var name = _idGenerator.GetId(typeof(ActionNode));

            var actionNode = new ActionNode(name, fn);
            _parentNodeStack.Peek().AddChild(actionNode);
            return this;
        }

        /// <summary>
        /// Like an action node... but the function can return true/false and is mapped to success/failure.
        /// </summary>
        public BehaviourTreeBuilder Condition(Func<TimeData, bool> fn)
        {
            return Do(t => fn(t) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure);
        }

        /// <summary>
        /// Create an inverter node that inverts the success/failure of its children.
        /// </summary>
        public BehaviourTreeBuilder Inverter()
        {
            AssertCanModify();
            var name = _idGenerator.GetId(typeof(InverterNode));

            var inverterNode = new InverterNode(name);

            if (_parentNodeStack.Count > 0)
            {
                _parentNodeStack.Peek().AddChild(inverterNode);
            }

            _parentNodeStack.Push(inverterNode);
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviourTreeBuilder Sequence()
        {
            AssertCanModify();
            var name = _idGenerator.GetId(typeof(SequenceNode));

            var sequenceNode = new SequenceNode(name);

            if (_parentNodeStack.Count > 0)
            {
                _parentNodeStack.Peek().AddChild(sequenceNode);
            }

            _parentNodeStack.Push(sequenceNode);
            return this;
        }

        /// <summary>
        /// Create a parallel node.
        /// </summary>
        public BehaviourTreeBuilder Parallel(int numRequiredToFail, int numRequiredToSucceed)
        {
            AssertCanModify();
            var name = _idGenerator.GetId(typeof(ParallelNode));

            var parallelNode = new ParallelNode(name, numRequiredToFail, numRequiredToSucceed);

            if (_parentNodeStack.Count > 0)
            {
                _parentNodeStack.Peek().AddChild(parallelNode);
            }

            _parentNodeStack.Push(parallelNode);
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviourTreeBuilder Selector()
        {
            AssertCanModify();
            var name = _idGenerator.GetId(typeof(SelectorNode));

            var selectorNode = new SelectorNode(name);

            if (_parentNodeStack.Count > 0)
            {
                _parentNodeStack.Peek().AddChild(selectorNode);
            }

            _parentNodeStack.Push(selectorNode);
            return this;
        }

        /// <summary>
        /// Splice a sub tree into the parent tree.
        /// </summary>
        public BehaviourTreeBuilder Splice(IBehaviourTreeNode subTree)
        {
            AssertCanSplice(subTree);

            _parentNodeStack.Peek().AddChild(subTree);
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public IBehaviourTreeNode Build()
        {
            AssertCanBuild();

            _isBuilt = true;
            return _curNode;
        }

        /// <summary>
        /// Ends a sequence of children.
        /// </summary>
        public BehaviourTreeBuilder End()
        {
            _curNode = _parentNodeStack.Pop();
            return this;
        }

        private void AssertCanModify()
        {
            if (_isBuilt)
            {
                throw new ApplicationException("Cannot modify or build already built tree");
            }
        }

        private void AssertCanAddLeaf()
        {
            AssertCanModify();

            if (_parentNodeStack.Count <= 0)
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

            if (_parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't splice an unnested sub-tree, there must be a parent-tree.");
            }
        }

        private void AssertCanBuild()
        {
            AssertCanModify();

            if (_curNode == null)
            {
                throw new ApplicationException("Can't create a behaviour tree with zero nodes");
            }
        }
    }
}