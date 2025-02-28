using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YP._02.Classes;

namespace YP._02.Context
{
    public class GroupsContext
    {
        public List<Groups> LoadGroups()
        {
            List<Groups> groups = new List<Groups>();
            string query = "SELECT * FROM `Groups`";
            using (var reader = Connection.Query(query))
            {
                while (reader.Read())
                {
                    groups.Add(new Groups
                    {
                        GroupId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
            return groups;
        }
        public bool Add(Groups group)
        {
            string query = $"INSERT INTO `Groups`(`Name`) VALUES ('{group.Name}')";
            var result = Connection.Query(query);
            return result != null;
        }
        public bool Update(Groups group)
        {
            string query = $"UPDATE `Groups` SET `Name`= '{group.Name}' WHERE GroupID = {group.GroupId}";
            var result = Connection.Query(query);
            return result != null;
        }
        public bool Delete(int groupId)
        {
            string query = $"DELETE FROM `Groups` WHERE `GroupID`= {groupId}";
            var result = Connection.Query(query);
            return result != null;
        }
    }
}
