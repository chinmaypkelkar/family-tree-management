using System.Collections.Generic;
using System.Linq;

namespace data_structures_assignment5
{
    public class WeightedMatrix
    {
        private const int MaxFamilyMembers = 100; // matrix row and column maximum length
        public Dictionary<string,List<FamilyTreeNode>> RelationshipManagementDictionary { get; set; } // dictionary to stores family. key is family memberid and value is list of nodes that family member is related. 
        public List<FamilyTreeNode> MembersList { get; set; } // stores all family members
        public string[,] AdjacencyMatrix { get; set; } // stores relations between members

        public WeightedMatrix()
        {
            AdjacencyMatrix = new string[MaxFamilyMembers,MaxFamilyMembers];
            RelationshipManagementDictionary = new Dictionary<string, List<FamilyTreeNode>>();
            MembersList = new List<FamilyTreeNode>();
        }

        // function to add family member
        public void AddNode(FamilyTreeNode familyMember)
        {
            MembersList.Add(familyMember); // adds node in the list
            RelationshipManagementDictionary.Add(familyMember.Id, new List<FamilyTreeNode>()); // creates entry in the dictionary
        }

        // function to add relation between two family members
        public void AddEditEdge(string familyMemberId1, string familyMemberId2, string relation)
        {
            var keys = GetSortedKeys(); // get all sorted keys for dictionary
            var familyMember1Index = keys.IndexOf(familyMemberId1);
            var familyMember2Index = keys.IndexOf(familyMemberId2);
            AdjacencyMatrix[familyMember1Index, familyMember2Index] = relation; // store relation in matrix 

            var relatedMembers = RelationshipManagementDictionary[familyMemberId1];
            if (relatedMembers.Find(x => x.Id == familyMemberId2) is null) // if there is no existing relation
            {
                RelationshipManagementDictionary[familyMemberId1].Add(MembersList.Find(x=>x.Id == familyMemberId2)); // add member to  dictionary
            }
            
            relatedMembers = RelationshipManagementDictionary[familyMemberId2];
            if (relatedMembers.Find(x => x.Id == familyMemberId1) is null) // if there is no existing relation
            {
                RelationshipManagementDictionary[familyMemberId2].Add(MembersList.Find(x=>x.Id == familyMemberId1)); // add member to dictionary
            }
        }
        
        // function to get sorted keys of dictionary
        private List<string> GetSortedKeys()
        {
            var keys = RelationshipManagementDictionary.Keys.ToList(); 
            keys.Sort();
            return keys;
        }
        

        // function to edit information of family member
        public void EditNode(string id, string firstName, string lastName, bool isMale, int age)
        {
            var existingMember = MembersList.Find(x => x.Id == id); // get existing member from memberlist
            existingMember.FirstName = firstName; // update information
            existingMember.LastName = lastName;
            existingMember.IsMale = isMale;
            existingMember.Age = age;
            
            // loop through values in the dictionary and update information of family member
            foreach (var member in from kv in RelationshipManagementDictionary from member in kv.Value where member.Id == id select member) 
            {
                member.FirstName = firstName;
                member.LastName = lastName;
                member.IsMale = isMale;
                member.Age = age;
            }
        }

        
        // function to delete family member
        public void DeleteNode(string memberId)
        {
            var keys = GetSortedKeys();
            var memberIndex = keys.IndexOf(memberId);
            for (var i = 0; i < AdjacencyMatrix.GetLength(0); i++) // iterate through columns and remove relation from matrix for member 
            {
                AdjacencyMatrix[memberIndex, i] = null;
            }
            
            for (var i = 0; i < AdjacencyMatrix.GetLength(1); i++) // iterate through rows and remove relation from matrix for member 
            {
                AdjacencyMatrix[i, memberIndex] = null;
            }

            RelationshipManagementDictionary[memberId] = null; // remove entry from dictionary
            foreach (var relations in RelationshipManagementDictionary.Values)  // loop through values in the dictionary and remove of family member
            {
                relations?.RemoveAll(x => x.Id == memberId);
            }

            MembersList.RemoveAll(x => x.Id == memberId); // remove from member list

        }
    }
}