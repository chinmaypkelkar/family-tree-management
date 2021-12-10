using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace data_structures_assignment5
{
    [ApiController]
    
    public class FamilyManagementController: ControllerBase
    {
        private readonly IFamilyTree _familyTree;
        public FamilyManagementController(IFamilyTree familyTree)
        {
            _familyTree = familyTree;
        }
        
        [HttpPost("[Controller]/[Action]")]
        public void AddFamilyMembers([FromBody] List<List<FamilyMember>> families) // Controller to add family members
        {
            foreach (var member in families.SelectMany(family => family))
            {
                _familyTree.AddFamilyMember(member);
            }
        }
        
        [HttpPost("[Controller]/[Action]")]
        public void AddEditRelations([FromBody] List<List<RelationViewModel>> familyRelations) // Controller to add relations
        {
            foreach (var relation in familyRelations.SelectMany(relations => relations))
            {
                _familyTree.CreateOrEditRelationShip(relation.MemberId1,relation.MemberId2,relation.Relation);
            }
        }
        
        [HttpPut("[Controller]/[Action]")]
        public void EditMember([FromBody] FamilyMember member) // Controller to edit member
        {
            _familyTree.EditMember(member);
        }

        [HttpDelete("[Controller]/[Action]")]
        public void DeleteMember([FromQuery] string memberId) // Controller to delete member
        {
            _familyTree.DeleteFamilyMember(memberId);
        }

        [HttpGet("[Controller]/[Action]")]
        public List<FamilyTreeNode> GetAllFamilyMembers() // Controller to get all family members
        {
            return _familyTree.GetAllFamilyMembers();
        }

        [HttpGet("[Controller]/[Action]")]
        public List<RelationViewModel> GetAllRelations() // Controller to get all family relations
        {
            return _familyTree.GetAllRelations();
        }
    }
}