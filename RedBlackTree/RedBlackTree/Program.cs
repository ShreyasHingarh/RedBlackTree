namespace RedBlackTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random gen = new Random(12);
            RedBlack<char> redblack = new RedBlack<char>();

            for (int i = 0; i < 15; i++)
            {
                redblack.Insert((char)(65+i));
            }
            ;
            Console.WriteLine(redblack.TreeValidation());
            ;

            redblack.Remove(redblack.Search('E'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('F'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('D'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('B'));
            Console.WriteLine(redblack.TreeValidation());
            ;
        }
    }
}