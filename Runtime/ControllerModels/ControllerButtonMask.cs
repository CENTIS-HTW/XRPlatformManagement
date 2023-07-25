namespace CENTIS.XRPlatform.ControllerModels
{
    [System.Flags]
    public enum ControllerButtonMask
    {
        None = 0,
        CompleteModel = 1,
        Body = 1 << 1,
        PrimaryButton = 1 << 2,
        SecondaryButton = 1 << 3,
        Trigger = 1 << 4,
        SystemButton = 1 << 5,
        ThumbStick = 1 << 6,
        Trackpad = 1 << 7,
        StatusLED = 1 << 8,
        GripButtonPrimary = 1 << 9,
        GripButtonSecondary = 1 << 10
    }
}