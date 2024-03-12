namespace Utility
{
    // Note: We order the y before the x to be consistent with latitude (y value) and longitude (x value). So, the highest bit is a y bit and the next highest is an x bit.

    public enum ProperMortonQuadrant : byte
    {
        YLowXLow,
        YLowXHigh,
        YHighXLow,
        YHighXHigh
    }
}
