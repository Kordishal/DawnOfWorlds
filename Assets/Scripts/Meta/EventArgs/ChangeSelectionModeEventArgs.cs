namespace Meta.EventArgs
{
    public class ChangeSelectionModeEventArgs : System.EventArgs
    {
        public SelectionMode OldMode { get; }
        public SelectionMode NewMode { get; }
            
        public ChangeSelectionModeEventArgs(SelectionMode oldMode, SelectionMode newMode)
        {
            OldMode = oldMode;
            NewMode = newMode;
        }
    }
}