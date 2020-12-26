using NExpect;
using static NExpect.Expectations;

using NUnit.Framework;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CaseHelperTests
    {
        [Test]
        public void CaseKebabCase_DoesConvert_PascalCaseToKebabCase()
        {
            var input = "PeopleTable";
            Expect(input.ConvertCase(Case.KebabCase)).To.Equal("people-table");
        }

        [Test]
        public void PascalCase_DoesConvert_ToCamelCase()
        {
            var input = "PeopleTable";
            Expect(input.ConvertCase(Case.CamelCase)).To.Equal("peopleTable");
        }

        public void SnakeCase_DoesConvert_ToCamelCase()
        {
            
        }
    }
}