namespace Report.Service.Exceptions
{
    /// <summary>
    /// Thrown when the vision model looked at the photo and declined to diagnose it
    /// (too dark, blurry, not a road, etc). This is an expected business outcome,
    /// not a server fault — it must never surface as a bare 500.
    /// </summary>
    public class PhotoRejectedException(string message) : Exception(message)
    {
        public string Code => "PHOTO_REJECTED";
    }
}