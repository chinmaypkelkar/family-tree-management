using System;
using System.Collections.Generic;
using System.Linq;

namespace data_structures_assignment5
{
    public class FamilyTree: IFamilyTree
    {
        private readonly WeightedMatrix _weightedMatrix; // backing graph structure

        public FamilyTree()
        {
            _weightedMatrix = new WeightedMatrix();
        }
       
        // function to add family member
        public void AddFamilyMember(FamilyMember member)
        {
            if (IsMemberPresent(member.Id)) // if family member is already present then don't add the same member
            {
                throw new Exception($"{member.Id} already exists");
            }
            var isMale = string.Equals(member.Gender, EGender.M.ToString(), StringComparison.InvariantCultureIgnoreCase); // convert gender received from user to appropriate boolean value of gender
            var newMember = new FamilyTreeNode(member.Id, member.FirstName,member.LastName,isMale, member.Age); // create new member
            _weightedMatrix.AddNode(newMember); // add to the graph
        }

        // function to check different constraints and add relation between family members
        public void CreateOrEditRelationShip(string familyMemberId1, string familyMemberId2, string relationShip)
        {
            if (!IsMemberPresent(familyMemberId1)) // if member1 is not present in the family, don't add relation
            {
                throw new Exception($"{familyMemberId1} doesn't exists");
            }

            if (!IsMemberPresent(familyMemberId2)) // if member2 is not present in the family, don't add relation
            {
                throw new Exception($"{familyMemberId2} doesn't exists");
            }

            CheckConstraints(familyMemberId1,familyMemberId2,relationShip);
            
            _weightedMatrix.AddEditEdge(familyMemberId1, familyMemberId2,relationShip); // add relation
        }
        

        // function to check if member is present in family
        private bool IsMemberPresent(string id)
        {
            return _weightedMatrix.RelationshipManagementDictionary.ContainsKey(id);
        }

        // function to check different constraints
        // 1. A member can't have any relation with himself/herself
        // 2. A member can't have multiple marriages at the same time
        // 3. A member can't marry with his/her sibling
        private void CheckConstraints(string familyMemberId1, string familyMemberId2, string relationShip)
        {
            // A member can't have any relation with himself/herself
            if (familyMemberId1 == familyMemberId2)
            {
               throw new Exception($"You can't have {relationShip} relation with yourself");
            }
            var keys = GetSortedKeys();
            var familyMember1Index = keys.IndexOf(familyMemberId1);
            var familyMember2Index = keys.IndexOf(familyMemberId2);
            var adjacencyMatrix = _weightedMatrix.AdjacencyMatrix;
            
            // member can't have multiple marriages
            if (string.Equals(relationShip,ERelation.Married.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                for (var i = 0; i < _weightedMatrix.AdjacencyMatrix.GetLength(0); i++)
                {
                    if (string.Equals(adjacencyMatrix[familyMember1Index, i],ERelation.Married.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    && i!= familyMember2Index)
                    {
                       throw new Exception($"{familyMemberId1} is already married. You can't marry multiple times");
                    }
                }
                
                for (var i = 0; i < _weightedMatrix.AdjacencyMatrix.GetLength(0); i++)
                {
                    if (string.Equals(adjacencyMatrix[familyMember2Index, i], ERelation.Married.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    &&  i!= familyMember1Index)
                    {
                        throw new Exception($"{familyMemberId2} is already married. You can't marry multiple times");
                    }
                }
            }

            // member can't marry sibling
            if (string.Equals(relationShip,ERelation.Married.ToString(), StringComparison.InvariantCultureIgnoreCase) && string.Equals(adjacencyMatrix[familyMember1Index,familyMember2Index], ERelation.Sibling.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"{familyMemberId1} and {familyMemberId2} are siblings. You can't marry your own sibling");
            }
            
        }
        
        // function to get sorted list of keys in family dictionary
        private List<string> GetSortedKeys()
        {
            var keys = _weightedMatrix.RelationshipManagementDictionary.Keys.ToList(); 
            keys.Sort();
            return keys;
        }

        // function to edit family member 
        public void EditMember(FamilyMember familyMember)
        {
            if (!IsMemberPresent(familyMember.Id)) // if member is not present, you can't edit 
            {
                throw new Exception($"{familyMember.Id} is not present in the family");
            }
            
            var isMale = string.Equals(familyMember.Gender, EGender.M.ToString(), StringComparison.InvariantCultureIgnoreCase);
            _weightedMatrix.EditNode(familyMember.Id, familyMember.FirstName, familyMember.LastName, isMale, familyMember.Age);
        }
        
        // function to delete family member
        public void DeleteFamilyMember(string memberId) 
        {
            if (!IsMemberPresent(memberId)) // if member is not present, you can't delete
            {
                throw new Exception($"{memberId} is not present in the family");
            }
            
            _weightedMatrix.DeleteNode(memberId);
        }

        // function to get all family members
        public List<FamilyTreeNode> GetAllFamilyMembers()
        {
            return _weightedMatrix.MembersList;
        }

        // function to get all relations
        public List<RelationViewModel> GetAllRelations()
        {
            var keys = GetSortedKeys();
            var relations = new List<RelationViewModel>();
            var adjacencyMatrix = _weightedMatrix.AdjacencyMatrix;
            for (var i = 0; i < adjacencyMatrix.GetLength(0); i++) // loop through matrix and populate list of relations
            {
                for (var j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j] != null) // if relation is present between members, add it in the list
                    {
                        relations.Add(new RelationViewModel
                        {
                            MemberId1 = keys[i],
                            MemberId2 = keys[j],
                            Relation = adjacencyMatrix[i, j]
                        });
                    }
                }
            }

            return relations;
        }
        
    }
}