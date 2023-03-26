namespace RedBlackTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random gen = new Random(12);
            RedBlack<int> redblack = new RedBlack<int>();
            for (int i = 0; i < 20; i++)
            {
                redblack.Insert(gen.Next(0,100));
            }
            ;
            Console.WriteLine(redblack.TreeValidation());
            ;
        }
    }
}