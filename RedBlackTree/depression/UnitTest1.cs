
using RedBlackTree;
namespace depression
{
    public class UnitTest1 
    {
        
        
        [Fact]
        public void DoesFailWhenNothingToInsert()
        {
            RedBlack<char> redblack = new RedBlack<char>();
            redblack.Insert('b');
            bool didFail = redblack.Remove(redblack.Search('A'));
            Assert.False(didFail);
        }
        [Fact]
        public void CanInsert() 
        {
            Random rand = new Random(2);
            RedBlack<int> red = new RedBlack<int>();
            for (int i = 0; i < 50; i++)
            {
                red.Insert(rand.Next(0,100));
            }
            Assert.True(red.TreeValidation());
        }
        [Fact]
        public void CanRemoveWithoutBreakingRandom() 
        {
            Random rand = new Random(2);
            List<int> ints = new List<int>();
            RedBlack<int> red = new RedBlack<int>();
            for (int i = 0; i < 50; i++)
            {
                int num = rand.Next(0, 100);
                red.Insert(num);
                ints.Add(num);
            }
            bool didFail = false;
            for (int i = 0; i < 50; i++)
            {
                if(i == 11)
                {

                }
                int index = rand.Next(0, ints.Count);
                red.Remove(red.Search(ints[index]));
                ints.Remove(ints[index]);
                if(!red.TreeValidation())
                {
                    didFail = true;
                }
            }
            Assert.False(didFail);

        }
    }
}