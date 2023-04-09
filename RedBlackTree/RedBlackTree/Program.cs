namespace RedBlackTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random gen = new Random(12);
            RedBlack<int> redblack = new RedBlack<int>();
            List<int> numsadded = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                int num = gen.Next(0, 100);
                if (numsadded.Contains(num)) continue;
                redblack.Insert(num);
                numsadded.Add(num);
            }
            ;
            Console.WriteLine(redblack.TreeValidation());
            ;
        }
    }
}