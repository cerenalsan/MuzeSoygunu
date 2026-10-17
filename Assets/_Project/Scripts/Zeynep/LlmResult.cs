namespace MuzeSoygunu.Llm
{
    // Bir LLM isteğinin sonucu: ya cevap metni ya da hata türü.
    public sealed class LlmResult
    {
        public bool IsSuccess { get; }
        public string Content { get; }
        public LlmError Error { get; }
        public string ErrorDetail { get; }

        private LlmResult(bool isSuccess, string content, LlmError error, string errorDetail)
        {
            IsSuccess = isSuccess;
            Content = content;
            Error = error;
            ErrorDetail = errorDetail;
        }

        public static LlmResult Success(string content)
        {
            return new LlmResult(true, content, LlmError.None, null);
        }

        public static LlmResult Failure(LlmError error, string detail = null)
        {
            return new LlmResult(false, null, error, detail);
        }
    }
}
