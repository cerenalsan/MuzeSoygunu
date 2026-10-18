namespace MuzeSoygunu.Llm
{
    // LLM isteğinin neden başarısız olduğunu belirtir.
    public enum LlmError
    {
        None,
        ServerUnreachable,
        Timeout,
        ModelNotFound,
        InvalidResponse
    }
}
