using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public enum TypeOfNode
    {
        TwoNode,
        ThreeNode,
        FourNode
    }
    public class Node<T>
    {
        public T Value;
        public Node<T> RightChild;
        public Node<T> LeftChild;
        public bool IsRed;
        public bool hasTwoChildren
        {
            get
            {
                if (RightChild != null && LeftChild != null)
                {
                    return true;
                }
                return false;
            }
        }
        public bool HasRight
        {
            get
            {
                if (RightChild == null) return false;
                return true;
            }
        }
        public bool HasLeft
        {
            get
            {
                if (LeftChild == null) return false;
                return true;
            }
        }
        public bool HasNoChildren
        {
            get
            {
                return RightChild == null && LeftChild == null;
            }
        }
        
        public Node(T value, bool isRed = true)
        {
            Value = value;
            IsRed = isRed;
        }
        
    }

    internal class RedBlack<T> where T : IComparable<T>
    {

        public Node<T> Root;
        void FlipColor(Node<T> node)
        {
            node.IsRed = true;
            node.LeftChild.IsRed = false;
            node.RightChild.IsRed = false;
        }
        void InsertValue(Node<T> Current, T Value)
        {
            if (Current.Value.CompareTo(Value) > 0)
            {
                Current.LeftChild = new Node<T>(Value);
            }
            else if (Current.Value.CompareTo(Value) <= 0)
            {
                Current.RightChild = new Node<T>(Value);
            }
        }
        Node<T> RotateLeft(Node<T> CurNode)
        {
            Node<T> temp = CurNode.RightChild;
            Node<T> temp2 = CurNode.RightChild.LeftChild;
            temp.LeftChild = CurNode;
            CurNode.RightChild = temp2;
            bool orig = temp.IsRed;
            temp.IsRed = CurNode.IsRed;
            CurNode.IsRed = orig;
            return temp;
        }
        Node<T> RotateRight(Node<T> CurNode)
        {
            Node<T> temp = CurNode.LeftChild;
            Node<T> temp2 = CurNode.LeftChild.RightChild;
            temp.RightChild = CurNode;
            CurNode.LeftChild = temp2;
            bool orig = temp.IsRed;
            temp.IsRed = CurNode.IsRed;
            CurNode.IsRed = orig;
            return temp;
        }
        private Node<T> GoThroughTree(Node<T> Current, T Value)
        {

            if (Current.IsRed == false && Current.HasLeft && Current.HasRight && Current.LeftChild.IsRed == true && Current.RightChild.IsRed == true)
            {
                FlipColor(Current);
            }
            if (Current.LeftChild != null && Current.Value.CompareTo(Value) > 0)
            {
                Current.LeftChild = GoThroughTree(Current.LeftChild, Value);
            }
            else if (Current.RightChild != null && Current.Value.CompareTo(Value) <= 0)
            {
                Current.RightChild = GoThroughTree(Current.RightChild, Value);
            }
            else
            {
                InsertValue(Current, Value);
                return Current;
            }
            //Rotating to Balance
            if (Current.HasRight && Current.RightChild.HasLeft && Current.RightChild.IsRed && Current.RightChild.LeftChild.IsRed)
            {
                Node<T> CopyOfCur = Current;
                Node<T> temp = Current.RightChild;
                Current.RightChild = null;
                temp.IsRed = Current.IsRed;
                Current.IsRed = !Current.IsRed;
                temp.LeftChild.LeftChild = Current;
                Current = temp;
                if (CopyOfCur == Root) Root = Current;
            }
            else if (Current.HasLeft && Current.LeftChild.HasRight && Current.LeftChild.IsRed && Current.LeftChild.RightChild.IsRed)
            {
                Node<T> CopyOfCur = Current;
                Node<T> temp = Current.LeftChild;
                Current.LeftChild = null;
                temp.IsRed = Current.IsRed;
                Current.IsRed = !Current.IsRed;
                temp.RightChild.RightChild = Current;
                Current = temp;
                if (CopyOfCur == Root) Root = Current;
            }
            if ((Current.HasRight && Current.RightChild.IsRed && Current.RightChild.HasRight && Current.RightChild.RightChild.IsRed)
                || Current.hasTwoChildren && Current.RightChild.IsRed && !Current.LeftChild.IsRed)  //Rotating To Be LeftLeaning
            {
                Node<T> CopyOfCur = Current;
                Current = RotateLeft(Current);

                if (CopyOfCur == Root)
                {
                    Root = Current;
                }
            }
            else if (Current.HasLeft && Current.LeftChild.LeftChild != null && Current.LeftChild.IsRed && Current.LeftChild.LeftChild.IsRed)
            {
                Node<T> CopyOfCur = Current;
                Current = RotateRight(Current);
                if (CopyOfCur == Root)
                {
                    Root = Current;
                }
            }


            return Current;
        }
        public void Insert(T value)
        {
            if (value.Equals(88))
            {
            }
            if (Root == null)
            {
                Root = new Node<T>(value, false);
                return;
            }
            Root = GoThroughTree(Root, value);
            if (Root.IsRed) Root.IsRed = false;
            if (!TreeValidation())
            {
                throw new Exception();
            }
        }
        private void FixUp()
        {

        }
        public void Remove(Node<T> node)
        {
            RecursiveRemove(Root,node);
            FixUp();
        }
        private void MoveRedLeft()
        {

        }
        private void MoveRedRight(Node<T> Current)
        {
            if(!Current.LeftChild.LeftChild.IsRed)
            {
                FlipColor(Current);
                return;
            }
            FlipColor(Current);
            Current = RotateRight(Current);
            FlipColor(Current);
        }
        private Node<T> FindMinimumNode(Node<T> Node)
        {
            
            while(Node.HasLeft)
            {
                Node = Node.LeftChild;
            }
            return Node;
        }
        private bool BSTDelete(Node<T> NodeToLookFor)
        {
            //MakeRecursively

            return true;
        }
        private bool RecursiveRemove(Node<T> Current, Node<T> NodeToLookFor)
        {
            bool DidRemove = false;
            //Go Left
            if (Current.HasLeft && NodeToLookFor.Value.CompareTo(Current.Value) < 0)
            {
                if(Current.HasLeft && Current.LeftChild.HasLeft && !Current.LeftChild.IsRed && !Current.LeftChild.LeftChild.IsRed)
                {
                    MoveRedLeft();
                }
                DidRemove = RecursiveRemove(Current.LeftChild, NodeToLookFor);
                
            }
            //Go Right or current is the one to delete
            else if (NodeToLookFor.Value.CompareTo(Current.Value) > 0 || Current == NodeToLookFor)
            {
                if(Current.HasLeft && Current.LeftChild.IsRed)
                {
                    Node<T> CopyOfCur = Current;
                    Current = RotateRight(Current);
                    if(CopyOfCur == Root)
                    {
                        Root = Current;
                    }
                }
                if (Current.HasNoChildren && Current == NodeToLookFor)
                {
                    return true;
                }
                //if value is still on the right
                if (Current.HasRight && NodeToLookFor.Value.CompareTo(Current.Value) > 0)
                {
                    if (Current.RightChild.HasLeft && Current.RightChild.LeftChild.HasLeft &&
                        !Current.RightChild.LeftChild.IsRed )// if right child is a 2-node
                    {
                        MoveRedRight();
                    }
                    DidRemove = RecursiveRemove(Current.RightChild, NodeToLookFor);
                }
                else
                {//In case of internal 3-node or 4-node
                    if (Current.RightChild.HasLeft && Current.RightChild.LeftChild.HasLeft &&
                            !Current.RightChild.LeftChild.IsRed && !Current.RightChild.LeftChild.LeftChild.IsRed)// if right child is a 2-node
                    {
                        MoveRedRight();
                    }

                    //Find Right SubTree's MinimumValue: From Current Go right once and than left till can't anymore
                    // replace the NodeToLookFor.Value with the nodelandedon.value 
                    Node<T> MinimumNode = FindMinimumNode(Current.RightChild);
                    T temp = NodeToLookFor.Value;
                    NodeToLookFor.Value = MinimumNode.Value;
                    MinimumNode.Value = temp;
                    
                    return BSTDelete(MinimumNode);
                }
            }
            //Remove the Node from its parent; 
            if(Current.HasLeft && Current.LeftChild == NodeToLookFor)
            {
                Current.LeftChild = null;
                return true;
            }
            else if (Current.HasRight && Current.RightChild == NodeToLookFor)
            {
                Current.RightChild = null;
                return true;
            }

            return DidRemove;
        }


        private bool RecursiveForFour(Node<T> Current, int TargetValue, int CurrentValue)
        {
            int cur = CurrentValue;
            bool IsEqual = false;
            if(Current.HasLeft)
            {
                if (!Current.LeftChild.IsRed) cur++;
                IsEqual = RecursiveForFour(Current.LeftChild, TargetValue, cur);
                cur = CurrentValue;
            }
            if(Current.HasRight)
            {
                if (!Current.RightChild.IsRed) cur++;
                IsEqual = RecursiveForFour(Current.RightChild, TargetValue, cur);
                cur = CurrentValue;
            }
            if(Current.HasNoChildren)
            {   
                IsEqual = cur == TargetValue;
            }
            else
            {
                cur--;
            }
            return IsEqual;
            
        }
        private bool RuleFourValidation()
        {
            //make this into recursion
            // figure out target value 
            //do recursion and compare current to target value
            int TargetValue = 0;
            Node<T> temp = Root;
            while(temp != null)
            {
                if(!temp.IsRed) TargetValue++;
                temp = temp.LeftChild;
            }
            return RecursiveForFour(Root,TargetValue,1);
        }
        private bool RuleThreeCheck()
        {
            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(Root);

            while (stack.Count != 0)
            {
                Node<T> current = stack.Pop();

                if (current.LeftChild != null)
                {
                    //Rule 3.If a node is red, then its children are black(therefore, red cannot touch red).
                    if (current.IsRed && current.LeftChild.IsRed) return false;
                    stack.Push(current.LeftChild);
                }
                if (current.RightChild != null)
                {
                    //Rule 3.If a node is red, then its children are black(therefore, red cannot touch red).
                    if (current.IsRed && current.RightChild.IsRed) return false;
                    stack.Push(current.RightChild);
                }
                //Rule 5 check
                if (current.hasTwoChildren && !current.IsRed && current.RightChild.IsRed && !current.LeftChild.IsRed) return false;
                
            }
            return true;
        }
        
        
        public bool TreeValidation()
        {
            bool Rule3 = RuleThreeCheck();
            bool Rule4 = RuleFourValidation();
            return !Root.IsRed && Rule3 && Rule4;
        }
        
    }
}
