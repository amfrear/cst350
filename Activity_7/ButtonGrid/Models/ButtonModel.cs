namespace ButtonGrid.Models
{
    public class ButtonModel
    {
        public int Id { get; set; }
        public int ButtonState { get; set; }
        public string ButtonImage { get; set; }

        // Parameterized constructor
        public ButtonModel(int id, int buttonState, string buttonImg)
        {
            Id = id;
            ButtonState = buttonState;
            ButtonImage = buttonImg;
        }

        // Default (empty) constructor
        public ButtonModel()
        {
        }
    }
}
