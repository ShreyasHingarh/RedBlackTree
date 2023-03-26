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
        public bool hasChildren
        {
            get
            {
                if (RightChild == null && LeftChild == null)
                {
                    return false;
                }
                return true;
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
        public Node(T value, bool isRed = true)
        {
            Value = value;
            IsRed = isRed;
        }
        //add someway of telling what node it is
        
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
            if ( Current.Value.CompareTo(Value) > 0)
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
                if (Root.IsRed) Root.IsRed = false;
            }
            if (Current.LeftChild != null && Current.Value.CompareTo(Value) > 0)
            {
                GoThroughTree(Current.LeftChild, Value);
            }
            else if (Current.RightChild != null && Current.Value.CompareTo(Value) <= 0)
            {
               GoThroughTree(Current.RightChild, Value);
            }
            else
            {
                InsertValue(Current, Value);
                return Current;
            }
            if (Current.RightChild != null && Current.RightChild.IsRed)
            {
                if (Current == Root)
                {
                    Current = RotateLeft(Current);
                    Root = Current;
                }
                else
                {
                    Current = RotateLeft(Current);
                }
            }
            if (Current.LeftChild != null && Current.LeftChild.IsRed && Current.LeftChild.LeftChild.IsRed)
            {
                if (Current == Root)
                {
                    Current = RotateRight(Current);
                    Root = Current;
                }
                else
                {
                    Current = RotateRight(Current);
                }
               
            }
            return Current;
        }
        public void Insert(T value)
        {
            if (Root == null)
            {
                Root = new Node<T>(value,false);
                return;
            }
            GoThroughTree(Root, value);
        }
        
        private bool RuleFourValidation()
        {
            int NumberToCheckAgainst = -1;
            int CurrentValue = 0;
            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(Root);


            while (stack.Count != 0)
            {
                Node<T> current = stack.Pop();
                bool hasRemoved = false;
                if (!current.IsRed)
                {
                    CurrentValue++;
                }
                if (current.LeftChild != null)
                {
                    stack.Push(current.LeftChild);
                }
                else
                {
                    if (NumberToCheckAgainst == -1)
                    {
                        NumberToCheckAgainst = CurrentValue;
                    }
                    else if (NumberToCheckAgainst != CurrentValue) return false;
                    if (!hasRemoved && !current.IsRed)
                    {
                        CurrentValue--;
                        hasRemoved = true;
                    }
                }
                if (current.RightChild != null)
                {
                    stack.Push(current.RightChild);
                }
                else
                {
                    if (NumberToCheckAgainst == -1)
                    {
                        NumberToCheckAgainst = CurrentValue;
                    }
                    else if (NumberToCheckAgainst != CurrentValue) return false;
                    if (!hasRemoved && !current.IsRed)
                    {
                        CurrentValue--;
                        hasRemoved = true;
                    }
                }

                
            }

            return true;
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
                if (current.hasChildren && !current.IsRed && current.RightChild.IsRed && !current.LeftChild.IsRed) return false;
                
            }
            return true;
        }
        
        
        public bool TreeValidation()
        {
            //Rule 6: nodes must be full    
            if (Root.IsRed) return false;
            if (!RuleThreeCheck()) return false;
            if (!RuleFourValidation()) return false;
            return true;
        }
        
    }
}
