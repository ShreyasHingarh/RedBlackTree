namespace RedBlackTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random gen = new Random(12);
            RedBlack<char> redblack = new RedBlack<char>();

            for (int i = 0; i < 26; i++)
            {
                redblack.Insert((char)(65+i));
            }
            ;
            Console.WriteLine(redblack.TreeValidation());
            ;

            redblack.Remove(redblack.Search('E'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('A'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('Y'));
            Console.WriteLine(redblack.TreeValidation());
            ;
            redblack.Remove(redblack.Search('L'));
            Console.WriteLine(redblack.TreeValidation());
            ;
        }
    }
}