namespace data_structures_assignment5
{
    public class FamilyTreeNode
    {
        public string Id { get; set; } // stores unique id
        public string FirstName { get; set; } // stores firstname
        public string LastName { get; set; } // stores lastname
        public int Age { get; set; } // stores Age
        public bool IsMale { get; set; } // stores boolean gender. male = true , female = false

        public FamilyTreeNode(string idIn, string firstNameIn, string lastNameIn, bool isMaleIn, int ageIn)
        {
            Id = idIn;
            FirstName = firstNameIn;
            LastName = lastNameIn;
            IsMale = isMaleIn;
            Age = ageIn;
        }
    }
}