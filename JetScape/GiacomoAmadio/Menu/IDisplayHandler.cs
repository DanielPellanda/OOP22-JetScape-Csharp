namespace JetScape.Menu
{
    // partial implementatation , the class for input managing has not been translated
    public interface IDisplayHandler
    {
        void GoUp();

        void GoDown();

        void Select();

        MenuOptions GetSelectedOption();
    }
}
