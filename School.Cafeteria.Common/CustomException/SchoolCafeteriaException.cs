using School.Cafeteria.Models.Firebase;

namespace School.Cafeteria.Common.CustomException
{
    public class SchoolCafeteriaException : Exception
    {
        public FirebaseError Error { get; set; }

        public SchoolCafeteriaException() : base() { }

        public SchoolCafeteriaException(FirebaseError error) : base(error.Error.Message)
        {
            this.Error = error;
        }
    }
}
