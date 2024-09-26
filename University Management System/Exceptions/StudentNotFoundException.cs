namespace University_Management_System.Exceptions
{
    [Serializable]
    internal class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(string? message) : base(message)
        {
        }
    }
}