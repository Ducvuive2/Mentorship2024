using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Data;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace DigitalAssetManagement
{
    public class DigitalAssetManagement
    {
        [Fact]
        public void InitialUser()
        {
            var user = new User();
            user.Name = "John";
            user.ID = 1;

            Assert.Equal(1, user.ID);
        }
        [Fact]
        public void InitialDriver()
        {
            var driver = new Driver();
            driver.ID = 1;
            driver.OwnerID = 1;
            driver.DriveName = "GoogleDrive";
            Assert.Equal(1, driver.OwnerID);
        }
        [Fact]
        public void TestUserHaveMultipleDriver()
        {
            var user = new User();
            user.Name = "John";
            user.ID = 1;

            var driver1 = new Driver();
            driver1.ID = 1;
            driver1.OwnerID = 1;
            driver1.DriveName = "GoogleDrive";


            var driver2 = new Driver();
            driver1.ID = 2;
            driver2.OwnerID = 1;
            driver2.DriveName = "OneDrive";

            // Assert
            Assert.Equal("GoogleDrive", driver1.DriveName);
        }
        [Fact]
        public void TestUserHavestore()
        {
            var user = new User();
            user.Name = "John";
            user.ID = 1;

            var driver1 = new Driver();
            var driver2 = new Driver();
            driver1.ID = 1;
            driver1.OwnerID = 1;
            driver1.DriveName = "GoogleDrive";

            driver1.ID = 2;
            driver2.OwnerID = 1;
            driver2.DriveName = "OneDrive";

            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 1, ParentStoreID = null });

            // Assert
            var hasstore = user.GetStores().Count > 0;
            Assert.True(hasstore);
        }
        [Fact]
        public void TestNumberStoreInDrive()
        {
            var user = new User();
            user.Name = "John";
            user.ID = 1;

            var driver1 = new Driver();
            driver1.ID = 1;
            driver1.OwnerID = 1;
            driver1.DriveName = "GoogleDrive";

            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 1, ParentStoreID = null });

            var driver2 = new Driver();
            driver2.ID = 2;
            driver2.OwnerID = 1;
            driver2.DriveName = "OneDrive";

            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 2, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 2, ParentStoreID = null });

            // Assert
            var googleDriveStores = user.GetStores().Where(f => f.ParentDriverID == driver1.ID).ToList();
            var oneDriveStores = user.GetStores().Where(f => f.ParentDriverID == driver2.ID).ToList();

            //Assert.Equal(2, googleDriveStores.Count);
            Assert.Equal(2, oneDriveStores.Count);
        }
        [Fact]
        public void TestInitialFile()
        {
            var user = new User();
            user.Name = "John";
            user.ID = 1;

            var driver1 = new Driver();
            driver1.ID = 1;
            driver1.OwnerID = 1;
            driver1.DriveName = "GoogleDrive";

            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.File, StoreName = "summary1.txt", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "summary2.txt", ParentDriverID = 1, ParentStoreID = null });

            var driver2 = new Driver();
            driver2.ID = 2;
            driver2.OwnerID = 1;
            driver2.DriveName = "OneDrive";

            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.File, StoreName = "summary3.txt", ParentDriverID = 2, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "summary4.txt", ParentDriverID = 2, ParentStoreID = null });

            var numberFileInGoogleDrive = user.GetStores().Where(f => f.ParentDriverID == driver2.ID && f.StoreType == StoreType.File).ToList();
            Assert.Equal(2, numberFileInGoogleDrive.Count);
        }
        [Fact]
        public void InitialPermission()
        {
            var permission = new Permission();
            permission.ID = 1;
            permission.DriverID = 1;
            permission.UserID = 1;
            permission.Roles = Role.Admin;

            Assert.Equal(1, permission.DriverID);
        }
        [Fact]
        public void SharePermission() 
        {
            var user = new User();
            var userFake = new User();
            user.Name = "John";
            user.ID = 1;

            userFake.Name = "Jerry";
            userFake.ID = 2;

            var driver1 = new Driver();
            driver1.ID = 1;
            driver1.OwnerID = 1;
            driver1.DriveName = "GoogleDrive";

            var driver2 = new Driver();
            driver2.ID = 2;
            driver2.OwnerID = 1;
            driver2.DriveName = "OneDrive";

            user.SharedPermissions(new Permission { ID = 1, DriverID = 1, StoreID = null, UserID = 2, Roles = Role.Admin });
            // Assert
            var jerryPermission = user.GetPermissionForUser(userFake.ID, driver1.ID);
            Assert.NotNull(jerryPermission);
            Assert.Equal(Role.Admin, jerryPermission?.Roles);
            Assert.Equal(driver1.ID, jerryPermission?.DriverID);
            Assert.Null(jerryPermission?.StoreID);
        }
    }


    public class Store
    {
        public int StoreID { get; set; }
        public StoreType StoreType { get; set; }
        public string StoreName { get; set; }
        public int ParentDriverID { get; set; }
        public int ? ParentStoreID { get; set; }
    }

    public class Permission
    {
        public Permission()
        {
        }

        public int ID { get; internal set; }
        public int ? DriverID { get; internal set; }
        public int ? StoreID { get; internal set; }
        public int UserID { get; internal set; }
        public Role Roles { get; internal set; }
    }
    public enum Role
    {
        Admin = 1,
        Contributor = 2,
        Reader = 3
    }

    public enum StoreType
    {
        Folder = 1,
        File = 2
    }

    internal class Driver
    {
        public Driver()
        {
        }

        public int ID { get; internal set; }
        public int OwnerID { get; internal set; }
        public string DriveName { get; internal set; }
    }

    public class User
    {
        public User()
        {
            Stores = new List<Store>();
        }
        public void AddToDrive(Store store)
        {
            Stores.Add(store);
        }
        public List<Store> GetStores()
        {
            return Stores;
        }
        public void SharedPermissions(Permission permission)
        {
            Permissions.Add(permission);
        }
        public Permission GetPermissionForUser(int userId, int driverId)
        {
            return Permissions.FirstOrDefault(p => p.UserID == userId && p.DriverID == driverId);
        }

        public string Name { get; internal set; }
        public int ID { get; internal set; }
        private List<Store> Stores { get; set; }
        private List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}