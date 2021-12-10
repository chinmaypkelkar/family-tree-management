using System.Collections.Generic;

namespace data_structures_assignment5
{
    // interface which is responsible for dependency injection
    public interface IFamilyTree
    {
        public void AddFamilyMember(FamilyMember member);
        public void CreateOrEditRelationShip(string familyMemberId1, string familyMemberId2, string relationShip);
        public void EditMember(FamilyMember member);
        void DeleteFamilyMember(string memberId);
        List<FamilyTreeNode> GetAllFamilyMembers();
        List<RelationViewModel> GetAllRelations();
    }
}