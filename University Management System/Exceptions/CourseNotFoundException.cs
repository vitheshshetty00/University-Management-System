﻿namespace University_Management_System.Exceptions
{
    [Serializable]
    internal class CourseNotFoundException(string? msg):Exception(msg)
    {
    }
}