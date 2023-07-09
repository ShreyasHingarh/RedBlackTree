using System;
using System.Collections;
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
        public bool HasOnlyRight
        {
            get
            {
                return HasRight && !HasLeft;
            }
        }
        public bool HasOnlyLeft
        {
            get
            {
                return !HasRight && HasLeft;
            }
        }
        public TypeOfNode NodeType
        {
            get
            {
                if (HasNoChildren) return TypeOfNode.Leaf;

                if (!IsRed && hasTwoChildren && !RightChild.IsRed && !LeftChild.IsRed)
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

    public class RedBlack<T> : ISortedSet<RedBlack<T>, T>
        where T : IComparable<T>
    {
        public int Count;
        public Node<T> Root;

        public IComparer<T> Comparer => Comparer<T>.Default;

        int ISortedSet<T>.Count => Count;
        #region Helper Funcs
        void FlipColor(Node<T> node)
        {
            node.IsRed = !node.IsRed;
            if (node.HasLeft)
            {
                node.LeftChild.IsRed = !node.LeftChild.IsRed;
            }
            if (node.HasRight)
            {
                node.RightChild.IsRed = !node.RightChild.IsRed;
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
            CurNode.IsRed = true;
            return temp;
        }
        Node<T> RotateRight(Node<T> CurNode)
        {
            Node<T> temp = CurNode.LeftChild;
            Node<T> temp2 = CurNode.LeftChild.RightChild;
            temp.RightChild = CurNode;
            CurNode.LeftChild = temp2;
            temp.IsRed = CurNode.IsRed;
            CurNode.IsRed = true;
            return temp;
        }
        public bool IsRed(Node<T> Node)
        {
            if (Node == null) return false;
            return Node.IsRed;
        }
        public Node<T> Search(T Value)
        {
            Node<T> cur = Root;
            if (Root.Value.Equals(Value))
            {
                return Root;
            }
            while (!cur.HasNoChildren)
            {
                if (cur.Value.Equals(Value))
                {
                    return cur;
                }
                if (cur.HasLeft && cur.Value.CompareTo(Value) > 0)
                {
                    cur = cur.LeftChild;
                }
                else if (cur.HasRight && cur.Value.CompareTo(Value) <= 0)
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
        private Node<T> MoveRedLeft(Node<T> Current)
        {
            FlipColor(Current);
            if (IsRed(Current.RightChild.LeftChild))
            {
                Current.RightChild = RotateRight(Current.RightChild);
                Current = RotateLeft(Current);
                FlipColor(Current);
                if (IsRed(Current.RightChild.RightChild))
                {
                    Current.RightChild = RotateLeft(Current.RightChild);
                }
            }
            return Current;
        }
        private Node<T> MoveRedRight(Node<T> Current)
        {
            FlipColor(Current);
            if (IsRed(Current.LeftChild.LeftChild))
            {
                Current = RotateRight(Current);
                FlipColor(Current);
            }

            return Current;
        }
        private Node<T> FindMinimumNode(Node<T> Node)
        {
            while (Node.HasLeft)
            {
                Node = Node.LeftChild;
            }
            return Node;
        }
        public List<T> InOrder()
        {
            List<T> temp = new List<T>();
            inorder(Root, temp);
            return temp;
        }
        private void inorder(Node<T> Current, List<T> Values)
        {
            if (Current == null) return;
            inorder(Current.LeftChild, Values);
            Values.Add(Current.Value);
            inorder(Current.RightChild, Values);
        }
        private Node<T> FixUp(Node<T> Current)
        {
            if (Current.NodeType != TypeOfNode.FourNode && IsRed(Current.RightChild))  //Rotating To Be LeftLeaning
            {
                Current = RotateLeft(Current);
            }
            if (IsRed(Current.LeftChild) && IsRed(Current.LeftChild.LeftChild))
            {
                Current = RotateRight(Current);
            }
            if (Current.NodeType == TypeOfNode.FourNode)
            {
                FlipColor(Current);
            }
            if (Current.HasLeft && IsRed(Current.LeftChild.RightChild) && !IsRed(Current.LeftChild.LeftChild))
            {
                Current.LeftChild = RotateLeft(Current.LeftChild);
                if (IsRed(Current.LeftChild))
                {
                    Current = RotateRight(Current);
                }
            }
            return Current;
        }
        #endregion 
        //Insert Functions
        private Node<T> GoThroughTree(Node<T> Current, T Value)
        {
            if(Current == null)
            {
                return new Node<T>(Value);
            }
            if (Current.NodeType == TypeOfNode.FourNode)
            {
                FlipColor(Current);
            }
            if (Current.Value.CompareTo(Value) > 0)
            {
                Current.LeftChild = GoThroughTree(Current.LeftChild, Value);
            }
            else if (Current.Value.CompareTo(Value) < 0)
            {
                Current.RightChild = GoThroughTree(Current.RightChild, Value);
            }
            //Rotation Checks
            if (Current.NodeType != TypeOfNode.FourNode && IsRed(Current.RightChild))  //Rotating To Be LeftLeaning
            {
                Current = RotateLeft(Current);
            }
            if (IsRed(Current.LeftChild) && IsRed(Current.LeftChild.LeftChild))
            {
                Current = RotateRight(Current);
            }

            return Current;
        }
        
        //Remove Functions
       
        private Node<T> RecursiveRemove(Node<T> Current, Node<T> NodeToLookFor)
        {
            //Go Left
            if (Current.HasLeft && NodeToLookFor.Value.CompareTo(Current.Value) < 0)
            {
                if (!IsRed(Current.LeftChild) && !IsRed(Current.LeftChild.LeftChild))
                {
                    Current = MoveRedLeft(Current);
                }
                Current.LeftChild = RecursiveRemove(Current.LeftChild, NodeToLookFor);
            }

            //Go Right or current is the one to delete
            else
            {
                if (IsRed(Current.LeftChild))
                {
                    Current = RotateRight(Current);
                }
                if (Current.HasNoChildren && Current == NodeToLookFor) return null;

                if (!IsRed(Current.RightChild) && !IsRed(Current.RightChild.LeftChild))// if right child is a 2-node
                {
                    Current = MoveRedRight(Current);
                }
                if (Current.HasRight && NodeToLookFor.Value.CompareTo(Current.Value) > 0)
                {   
                    Current.RightChild = RecursiveRemove(Current.RightChild, NodeToLookFor);
                }
                else 
                {
                    Node<T> MinimumNode = FindMinimumNode(Current.RightChild);
                    T temp = NodeToLookFor.Value;
                    NodeToLookFor.Value = MinimumNode.Value;
                    MinimumNode.Value = temp;

                    Current.RightChild = RecursiveRemove(Current.RightChild, MinimumNode);

                    return FixUp(Current);
                }
            }
            return FixUp(Current);
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
            while (temp != null)
            {
                if (!temp.IsRed) TargetValue++;
                if (temp.HasLeft)
                {
                    temp = temp.LeftChild;
                }
                else
                {
                    temp = temp.RightChild;
                }

            }
            bool thing = RecursiveForFour(Root, TargetValue, 0);
            if (!thing)
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
            if (Root == null && Count == 0) return true;

            bool Rule3 = RuleThreeCheck();
            ;
            bool Rule4 = RuleFourValidation();
            ;
            if (!Rule3)
            {

            }
            if (!Rule4)
            {

            }
            return !Root.IsRed && Rule3 && Rule4;
        }
        public void Clear()
        {
            Root = null;
        }

        public bool Add(T item)
        {
            if (Root == null)
            {
                Root = new Node<T>(item, false);
                Count++;
                return true;
            }
            Root = GoThroughTree(Root, item);
            Count++;
            Root.IsRed = false;
            return true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                Add(item);
            }
        }

        bool ISortedSet<T>.Contains(T item)
        {
            return Search(item) != null;
        }

        public bool Remove(T item)
        {
            Node<T> temp = Search(item);
            if (temp != null)
            {
                Root = RecursiveRemove(Root, temp);
                if (Root != null) Root.IsRed = false;
                Count--;
                return true;
            }
            return false;
        }

        public T Max()
        {
            Node<T> temp = Root;
            while (temp.HasRight)
            {
                temp = temp.RightChild;
            }
            return temp.Value;
        }

        public T Min()
        {
            Node<T> temp = FindMinimumNode(Root);
            return temp.Value;
        }

        private Node<T> ceiling(Node<T> Current, T item, Node<T> Previous)
        {
            if (Current.Value.Equals(item)) return Current;
            if (Current.HasLeft && item.CompareTo(Current.Value) < 0)
            {
                ceiling(Current.LeftChild, item, Current);
            }
            else if(Current.HasNoChildren)
            {
                return Current;
            }

        }
        public T Ceiling(T item)
        {
            if (Max().CompareTo(item) < 0)
            {
                return Max();
            }
            return ceiling(Root, item,Root).Value;
        }
        
        public T Floor(T item)
        {
            throw new NotImplementedException();
        }

        ISortedSet<T> ISortedSet<T>.Union(ISortedSet<T> other) => Union(other);

        ISortedSet<T> ISortedSet<T>.Intersection(ISortedSet<T> other) => Intersection(other);

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public RedBlack<T> Union(ISortedSet<T> other)
        {
            RedBlack<T> values = new RedBlack<T>();
            List<T> nodes = InOrder();
            AddRange(nodes);
            AddRange(other);
            return values;
        }

        public RedBlack<T> Intersection(ISortedSet<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
/*
 * 
class Hooray : IEnumerable<int> // > array
{
    public struct Enumerator : IEnumerator<int>
    {
        int state;
        Hooray hiphip;
        
        public Enumerator(Hooray myHooray)
        {
            state = 0;
            hiphip = myHooray;
        }

        public int Current {get; private set;}

        object IEnumerator.Current => Current;

        public void Dispose() {}        

        public bool MoveNext()
        {
            switch (++state)
            {
                case 1:
                    Current = hiphip.item1;
                    break;
                case 2:
                    Current = hiphip.item2;
                    break;
                case 3:
                    Current = hiphip.item3;
                    break;
                case 4:
                    Current = hiphip.item4;
                    break;
                case 5:
                    Current = hiphip.item5;
                    break;
                default: return false;                    
            }
            return true;
        }

        public void Reset()
        {
            state = 0;
        }
    }
    
    int item1;
    int item2;
    int item3;
    int item4;
    int item5;
       
    public Hooray(int i1, int i2, int i3, int i4, int i5)
    {
        item1 = i1;
        item2 = i2;
        item3 = i3;
        item4 = i4;
        item5 = i5;
    }
    
    public IEnumerator<int> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class C {
    
    public static void Main() 
    {
        Hooray hip = new Hooray(5, 10, 15, 20, 25);
        var a = hip.GetEnumerator();
        while (a.MoveNext())
        {
            var number = a.Current;
            Console.WriteLine(number);
        }
        
        foreach (var item in hip)
        {
            Console.WriteLine(item)   
        }
            
    }
}
 * */
