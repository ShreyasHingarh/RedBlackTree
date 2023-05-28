using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedBlackTree
{
    public enum TypeOfNode
    {
        TwoNode,
        ThreeNode,
        FourNode,
        Red,
        Leaf
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
       
        public TypeOfNode NodeType 
        {
            get
            {
                if (HasNoChildren) return TypeOfNode.Leaf;
                
                if (hasTwoChildren && !RightChild.IsRed && !LeftChild.IsRed)
                {
                    return TypeOfNode.TwoNode;
                }
                if (hasTwoChildren && RightChild.IsRed && LeftChild.IsRed)
                {
                    return TypeOfNode.FourNode;
                }
                if (IsRed) return TypeOfNode.Red;
                return TypeOfNode.ThreeNode;
            }
        }
        public Node(T value, bool isRed = true)
        {
            Value = value;
            IsRed = isRed;
        }
        
    }

    public class RedBlack<T> where T : IComparable<T>
    {
        public int Count;
        public Node<T> Root;
        void FlipColor(Node<T> node)
        {
            node.IsRed = !node.IsRed;
            if(node.HasLeft)
            {
                node.LeftChild.IsRed = !node.LeftChild.IsRed;
            }
            if(node.HasRight)
            {
                node.RightChild.IsRed = !node.RightChild.IsRed;
            }
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
        private Node<T> RotatingChecks(Node<T> Current)
        {

            if (Current.HasRight && !Current.HasLeft && Current.RightChild.HasLeft && !Current.RightChild.HasRight)
            {
                Current.RightChild = RotateRight(Current.RightChild);
                Current = RotateLeft(Current);
                if (Current.RightChild.IsRed)
                {
                    Current.RightChild.IsRed = false;
                }
            }
            if (Current.NodeType != TypeOfNode.FourNode && Current.HasRight && Current.RightChild.IsRed)  //Rotating To Be LeftLeaning
            {
                Current = RotateLeft(Current);
            }
            if (Current.HasLeft && Current.LeftChild.LeftChild != null && Current.LeftChild.IsRed && Current.LeftChild.LeftChild.IsRed)
            {
                Current = RotateRight(Current);
            }
            return Current;
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
                Current = RotatingChecks(Current);
                return Current;
            }
            //Rotating to Balance
            Current = RotatingChecks(Current);


            return Current;
        }
        public void Insert(T value)
        {
            if (Root == null)
            {
                Root = new Node<T>(value, false);
                Count++;
                return;
            }
            Root = GoThroughTree(Root, value);
            Count++;
            if (Root.IsRed) Root.IsRed = false;
            if (!TreeValidation())
            {
                //throw new Exception();
            }
        }
        private Node<T> FixUp(Node<T> Current)
        {
            
            if(Current.HasRight && Current.IsRed && !Current.RightChild.IsRed && !Current.HasLeft && Current.RightChild.HasNoChildren)
            {
                FlipColor(Current);
            }
            if (Current.NodeType == TypeOfNode.FourNode)
            {
                FlipColor(Current);
            }
            //Enforce Left Leaning Policy: See a red node on the right? Rotate left!
            if (Current.NodeType != TypeOfNode.FourNode && Current.HasRight && Current.RightChild.IsRed)
            {
                Current = RotateLeft(Current);   
            }
            
            //Balance 4-nodes
            Current = RotatingChecks(Current);
            //Break Up 4-Nodes **********************Check.NodeType ************************
            
            //Another check for right leaning nodes
            if(Current.HasLeft && Current.LeftChild.HasRight && Current.LeftChild.RightChild.IsRed)
            {
                Current.LeftChild = RotateLeft(Current.LeftChild);
            }
            
            return Current;
        }
        public Node<T> Search(T Value)
        {
            Node<T> cur = Root;
            if(Root.Value.Equals(Value))
            {
                return Root;
            }
            while(!cur.HasNoChildren)
            {
                if(cur.Value.Equals(Value))
                {
                    return cur;
                }
                if(cur.HasLeft && cur.Value.CompareTo(Value) > 0)
                {
                    cur = cur.LeftChild;
                }
                else if(cur.HasRight && cur.Value.CompareTo(Value) <= 0)
                {
                    cur = cur.RightChild; 
                }
            }
            if (cur.Value.Equals(Value))
            {
                return cur;
            }
            return null;
        }
        private bool Contains(T Value)
        {
            return Search(Value) != null;
        }
        public bool Remove(Node<T> node)
        {
            if (node != null)
            {
                Root = RecursiveRemove(Root, node);
                if (Root.IsRed) Root.IsRed = false;
                Count--;
                return true;
            }
            return false;
        }
        private Node<T> MoveRedLeft(Node<T> Current)
        {
            FlipColor(Current);
            if(Current.HasRight && Current.RightChild.HasLeft && Current.RightChild.IsRed && Current.RightChild.LeftChild.IsRed)
            {
                Current.RightChild = RotateRight(Current.RightChild);
                Current = RotateLeft(Current);
                FlipColor(Current);
                if(Current.RightChild.RightChild.IsRed)
                {
                    Current.RightChild = RotateLeft(Current.RightChild);
                }
            }
            return Current;
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
        private bool RecursiveBSTDelete(Node<T> NodeToLookFor, Node<T> Current)
        {
            if(Current.HasLeft && Current.LeftChild != NodeToLookFor)
            {
                bool thing = RecursiveBSTDelete(NodeToLookFor, Current.LeftChild);
                Current = FixUp(Current);
                return thing;
            }
            else if(Current.LeftChild == null)
            {
                return false;
            }
            Current.LeftChild = null;
            return true;
        }
              
        private Node<T> RecursiveRemove(Node<T> Current, Node<T> NodeToLookFor)
        {
            //Go Left
            if (Current.HasLeft && NodeToLookFor.Value.CompareTo(Current.Value) < 0)
            {
                // CHECK .NodeType
                if(Current.LeftChild.NodeType == TypeOfNode.TwoNode)
                {
                    Current = MoveRedLeft(Current);
                }
                Current.LeftChild = RecursiveRemove(Current.LeftChild, NodeToLookFor);
            }

            //Go Right or current is the one to delete
            else if (NodeToLookFor.Value.CompareTo(Current.Value) > 0 || Current == NodeToLookFor)
            {
                if(Current.HasLeft && Current.LeftChild.LeftChild != null && Current.LeftChild.IsRed && Current.LeftChild.LeftChild.IsRed)
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
                    return null;
                }
                
                if (Current.HasRight && NodeToLookFor.Value.CompareTo(Current.Value) > 0)
                {
                    if (Current.RightChild.NodeType == TypeOfNode.TwoNode)// if right child is a 2-node
                    {
                        MoveRedRight(Current);
                    }
                    Current.RightChild = RecursiveRemove(Current.RightChild, NodeToLookFor);
                }
                else
                {
                    if (Current.RightChild.NodeType == TypeOfNode.TwoNode)// if right child is a 2-node
                    {
                        MoveRedRight(Current);
                    }

                    //Find Right SubTree's MinimumValue: From Current Go right once and than left till can't anymore
                    
                    Node<T> MinimumNode = FindMinimumNode(Current.RightChild);
                    T temp = NodeToLookFor.Value;
                    NodeToLookFor.Value = MinimumNode.Value;
                    MinimumNode.Value = temp;
                    NodeToLookFor = MinimumNode;
                    if (Current.RightChild == NodeToLookFor)
                    {
                        Current.RightChild = null;
                    }
                    else
                    {
                        //Problem: Goes back to root once done removing the previous root value, Should go to H first and rotate then.
                        RecursiveBSTDelete(MinimumNode, Current.RightChild);
                        Current.RightChild = FixUp(Current.RightChild);
                    }
                    Current = FixUp(Current);
                    return Current;
                }
            }
            Current = FixUp(Current);
            return Current;
        }

        //Below is the validation functions
        private bool RecursiveForFour(Node<T> Current, int TargetValue, int CurrentValue)
        {
            if (!Current.IsRed)
            {
                CurrentValue++;
            }
            if (Current.HasNoChildren)
            {
                return TargetValue == CurrentValue;
            }
            
            bool one = true;
            bool two = true;
            if (Current.HasLeft)
            {
                one = RecursiveForFour(Current.LeftChild, TargetValue, CurrentValue);
            }
            if (Current.HasRight) 
            {
                two = RecursiveForFour(Current.RightChild, TargetValue, CurrentValue);
            }
            return one && two;
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
                if(temp.HasLeft)
                {
                    temp = temp.LeftChild;
                }
                else
                {
                    temp = temp.RightChild;
                }

            }
            bool thing = RecursiveForFour(Root, TargetValue, 0);
            if(!thing)
            {

            }
            return thing;
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
                    if (current.IsRed && current.LeftChild.IsRed)
                    {
                        return false;
                    }
                    stack.Push(current.LeftChild);
                }
                if (current.RightChild != null)
                {
                    //Rule 3.If a node is red, then its children are black(therefore, red cannot touch red).
                    if (current.IsRed && current.RightChild.IsRed) 
                    {
                        return false;
                    }
                    stack.Push(current.RightChild);
                }
                //Rule 5 check if 3 NODES are rightleaning
                if (((!current.HasLeft) || (current.HasLeft && !current.LeftChild.IsRed)) && current.HasRight && current.RightChild.IsRed)
                {
                    return false;   
                }
                
            }
            return true;
        }
        
        
        public bool TreeValidation()
        {
            bool Rule3 = RuleThreeCheck(); 
            ;
            bool Rule4 = RuleFourValidation();
            ;
            if(!Rule3)
            {

            }
            if(!Rule4)
            {

            }
            return !Root.IsRed && Rule3 && Rule4;
        }
        
    }
}
