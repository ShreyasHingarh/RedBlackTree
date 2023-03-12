using System;
using System.Collections.Generic;
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
        public bool hasChildren
        {
            get
            {
                if(RightChild == null && LeftChild == null)
                {
                    return false;
                }
                return true;
            }
        }
        public Node(T value)
        {
            Value = value;
            IsRed = true;
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
            if (Current.LeftChild != null && Current.Value.CompareTo(Value) == -1)
            {
                Current.LeftChild = new Node<T>(Value);
            }
            else if (Current.RightChild != null && Current.Value.CompareTo(Value) <= 0)
            {
                Current.RightChild = new Node<T>(Value);
            }
        }
        void RotateLeft(Node<T> CurNode)
        {
            CurNode.RightChild.IsRed = false;
            CurNode.IsRed = true;
            Node<T> ChildOfRight = CurNode.RightChild.LeftChild;
            CurNode.RightChild.LeftChild = CurNode;
            CurNode.RightChild = ChildOfRight;
        }
        void RotateRight(Node<T> CurNode)
        {
            CurNode.LeftChild.IsRed = false;
            CurNode.IsRed = true;
            Node<T> ChildOfLeft = CurNode.LeftChild.RightChild;
            CurNode.LeftChild.RightChild = CurNode;
            CurNode.LeftChild = ChildOfLeft;
        }
        public Node<T> GoThroughTree(Node<T> Current, T Value)
        {
            if (Current.IsRed == false && Current.LeftChild.IsRed == true && Current.RightChild.IsRed == false)
            {
                FlipColor(Current);
            }
            if (Current.LeftChild != null && Current.Value.CompareTo(Value) == -1)
            {
                Current = Current.LeftChild;
                GoThroughTree(Current, Value);
            }
            else if(Current.RightChild != null && Current.Value.CompareTo(Value) <= 0)
            {
                Current = Current.RightChild;
                GoThroughTree(Current, Value);
            }
            else
            {
                InsertValue(Current,  Value);
                return Current;
            }
            if(Current.RightChild != null && Current.RightChild.IsRed)
            {
                RotateLeft(Current);
                if(Current.LeftChild.IsRed && Current.LeftChild.LeftChild.IsRed)
                {
                    RotateRight(Current);
                }
            }
            return Current;
        }
        public void Insert(T value)
        {
            if(Root == null)
            {
                Root = new Node<T>(value);
                return;
            }
            GoThroughTree(Root, value);
        }
    }
}
