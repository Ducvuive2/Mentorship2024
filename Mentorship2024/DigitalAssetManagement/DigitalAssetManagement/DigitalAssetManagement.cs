using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DigitalAssetManagement
{
    public class DigitalAssetManagementTests
    {
        [Fact]
        public void InitialUser()
        {
            var user = new UserBuilder().WithName("John").WithID(1).Build();

            Assert.Equal(1, user.ID);
        }

        [Fact]
        public void InitialDriver()
        {
            var driver = new Driver { ID = 1, OwnerID = 1, DriveName = "GoogleDrive" };

            Assert.Equal(1, driver.OwnerID);
        }

        [Fact]
        public void TestUserHaveMultipleDrivers()
        {
            var user = new UserBuilder().WithName("John").WithID(1).Build();

            var driver1 = new Driver { ID = 1, OwnerID = 1, DriveName = "GoogleDrive" };
            var driver2 = new Driver { ID = 2, OwnerID = 1, DriveName = "OneDrive" };

            // Assert
            Assert.Equal("GoogleDrive", driver1.DriveName);
            Assert.Equal("OneDrive", driver2.DriveName);
        }

        [Fact]
        public void TestUserHasStores()
        {
            var user = new UserBuilder()
                .WithName("John")
                .WithID(1)
                .AddStore(new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 1, ParentStoreID = null })
                .AddStore(new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 1, ParentStoreID = null })
                .Build();

            // Assert
            var hasStore = user.GetStores().Any();
            Assert.True(hasStore);
        }

        [Fact]
        public void TestNumberOfStoresInDrive()
        {
            var user = new UserBuilder()
                .WithName("John")
                .WithID(1)
                .AddStore(new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 1, ParentStoreID = null })
                .AddStore(new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 1, ParentStoreID = null })
                .AddStore(new Store { StoreID = 3, StoreType = StoreType.Folder, StoreName = "Project1", ParentDriverID = 2, ParentStoreID = null })
                .AddStore(new Store { StoreID = 4, StoreType = StoreType.Folder, StoreName = "Project2", ParentDriverID = 2, ParentStoreID = null })
                .Build();

            // Assert
            var googleDriveStores = user.GetStores().Where(f => f.ParentDriverID == 1).ToList();
            var oneDriveStores = user.GetStores().Where(f => f.ParentDriverID == 2).ToList();

            Assert.Equal(2, googleDriveStores.Count);
            Assert.Equal(2, oneDriveStores.Count);
        }

        [Fact]
        public void TestInitialFile()
        {
            var user = new UserBuilder()
                .WithName("John")
                .WithID(1)
                .AddStore(new Store { StoreID = 1, StoreType = StoreType.File, StoreName = "summary1.txt", ParentDriverID = 1, ParentStoreID = null })
                .AddStore(new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "summary2.txt", ParentDriverID = 1, ParentStoreID = null })
                .AddStore(new Store { StoreID = 3, StoreType = StoreType.File, StoreName = "summary3.txt", ParentDriverID = 2, ParentStoreID = null })
                .AddStore(new Store { StoreID = 4, StoreType = StoreType.File, StoreName = "summary4.txt", ParentDriverID = 2, ParentStoreID = null })
                .Build();

            var numberFileInGoogleDrive = user.GetStores().Where(f => f.ParentDriverID == 1 && f.StoreType == StoreType.File).ToList();
            Assert.Equal(2, numberFileInGoogleDrive.Count);
        }

        [Fact]
        public void InitialPermission()
        {
            var permission = new Permission { ID = 1, DriverID = 1, UserID = 1, Roles = Role.Admin };

            Assert.Equal(1, permission.DriverID);
        }

        [Fact]
        public void SharePermission()
        {
            var user = new UserBuilder().WithName("John").WithID(1).Build();
            var userFake = new UserBuilder().WithName("Jerry").WithID(2).Build();

            user.Permissions.Add(new Permission { ID = 1, DriverID = 1, StoreID = null, UserID = 2, Roles = Role.Admin });

            // Assert
            var jerryPermission = user.GetPermissionForUserInDriver(userFake.ID, 1);
            Assert.NotNull(jerryPermission);
            Assert.Equal(Role.Admin, jerryPermission?.Roles);
            Assert.Equal(1, jerryPermission?.DriverID);
            Assert.Null(jerryPermission?.StoreID);
        }
        [Fact]
        public void AdminUserSharesContributorPermissions()
        {
            // Arrange
            var adminUser = new UserBuilder()
                .WithName("admin_user")
                .WithID(1)
                .Build();

            var contributorUser = new UserBuilder()
                .WithName("contributor1")
                .WithID(2)
                .Build();

            var rootFolder = new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Root1", ParentDriverID = 1, ParentStoreID = null };
            var subFolder = new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "SubFolder1", ParentDriverID = 1, ParentStoreID = 1 };

            adminUser.AddToDrive(rootFolder);
            adminUser.AddToDrive(subFolder);

            var adminPermission = new Permission { ID = 1, DriverID = 1, StoreID = 1, UserID = 1, Roles = Role.Admin };
            adminUser.Permissions.Add(adminPermission); // Admin user gives themselves admin permission

            // Act
            var contributorPermission = new Permission { ID = 2, DriverID = null, StoreID = 2, UserID = 2, Roles = Role.Contributor };
            adminUser.SharePermissions(contributorPermission, contributorUser);

            // Assert
            var sharedPermission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, subFolder.StoreID);

            Assert.NotNull(sharedPermission);
            Assert.Equal(Role.Contributor, sharedPermission?.Roles);
            Assert.Equal(subFolder.StoreID, sharedPermission?.StoreID);

            // Checking if the contributor has the right permissions
            Assert.True(sharedPermission?.CanAdd() ?? false);
            Assert.True(sharedPermission?.CanModify() ?? false);
            Assert.True(sharedPermission?.CanDelete() ?? false);
        }
        [Fact]
        public void ContributorDontHavePermissionInParentFolder()
        {
            // Arrange
            var adminUser = new UserBuilder()
                .WithName("admin_user")
                .WithID(1)
                .Build();

            var contributorUser = new UserBuilder()
                .WithName("contributor1")
                .WithID(2)
                .Build();

            var rootFolder = new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Root1", ParentDriverID = 1, ParentStoreID = null };
            var subFolder = new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "SubFolder1", ParentDriverID = 1, ParentStoreID = 1 };

            adminUser.AddToDrive(rootFolder);
            adminUser.AddToDrive(subFolder);

            var adminPermission = new Permission { ID = 1, DriverID = 1, StoreID = 1, UserID = 1, Roles = Role.Admin };
            adminUser.Permissions.Add(adminPermission); // Admin user gives themselves admin permission

            // Act
            var contributorPermission = new Permission { ID = 2, DriverID = null, StoreID = 2, UserID = 2, Roles = Role.Contributor };
            adminUser.SharePermissions(contributorPermission, contributorUser);

            // Assert
            var sharedPermission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, rootFolder.StoreID);

            Assert.Null(sharedPermission);

            // Checking if the contributor has the right permissions
            Assert.False(sharedPermission?.CanAdd() ?? false);
            Assert.False(sharedPermission?.CanModify() ?? false);
            Assert.False(sharedPermission?.CanDelete() ?? false);
        }
        [Fact]
        public void ContributorPermissionInheritedToSubFolders()
        {
            // Arrange
            var adminUser = new UserBuilder()
                .WithName("admin_user")
                .WithID(1)
                .Build();

            var contributorUser = new UserBuilder()
                .WithName("contributor1")
                .WithID(2)
                .Build();

            var rootFolder = new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Root1", ParentDriverID = 1, ParentStoreID = null };
            var subFolder1 = new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "SubFolder1", ParentDriverID = 1, ParentStoreID = 1 };
            var subFolder2 = new Store { StoreID = 3, StoreType = StoreType.Folder, StoreName = "SubFolder2", ParentDriverID = 1, ParentStoreID = 2 };

            adminUser.AddToDrive(rootFolder);
            adminUser.AddToDrive(subFolder1);
            adminUser.AddToDrive(subFolder2);

            var adminPermission = new Permission { ID = 1, DriverID = 1, StoreID = 1, UserID = 1, Roles = Role.Admin };
            adminUser.Permissions.Add(adminPermission); // Admin user gives themselves admin permission

            // Act: Grant Contributor role on Root1
            var contributorPermission = new Permission { ID = 2, DriverID = null, StoreID = 1, UserID = 2, Roles = Role.Contributor };
            adminUser.SharePermissions(contributorPermission, contributorUser);

            // Assert

            // Check SubFolder2
            var subFolder2Permission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, subFolder2.StoreID);
            Assert.NotNull(subFolder2Permission);
            Assert.Equal(Role.Contributor, subFolder2Permission?.Roles);
            Assert.True(subFolder2Permission?.CanAdd());
            Assert.True(subFolder2Permission?.CanModify());
            Assert.True(subFolder2Permission?.CanDelete());
        }

    }

    public class Store
    {
        public int StoreID { get; set; }
        public StoreType StoreType { get; set; }
        public string StoreName { get; set; }
        public int ParentDriverID { get; set; }
        public int? ParentStoreID { get; set; }
    }

    public class Permission
    {
        public int ID { get; set; }
        public int? DriverID { get; set; }
        public int? StoreID { get; set; }
        public int UserID { get; set; }
        public Role Roles { get; set; }
        public bool CanAdd()
        {
            return Roles == Role.Admin || Roles == Role.Contributor;
        }

        public bool CanModify()
        {
            return Roles == Role.Admin || Roles == Role.Contributor;
        }

        public bool CanDelete()
        {
            return Roles == Role.Admin || Roles == Role.Contributor;
        }

        public bool CanView()
        {
            return Roles == Role.Admin || Roles == Role.Contributor || Roles == Role.Reader;
        }
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

    public class Driver
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public string DriveName { get; set; }
    }

    public class User
    {
        public User()
        {
            Stores = new List<Store>();
            Permissions = new List<Permission>();
        }

        public void AddToDrive(Store store)
        {
            Stores.Add(store);
        }

        public List<Store> GetStores()
        {
            return Stores;
        }

        public void SharePermissions(Permission permission, User targetUser)
        {
            // If the permission already exists for the target user, update it; otherwise, add it.
            var existingPermission = targetUser.Permissions.FirstOrDefault(p => p.UserID == permission.UserID && p.StoreID == permission.StoreID);

            if (existingPermission != null)
            {
                existingPermission.Roles = permission.Roles;
            }
            else
            {
                targetUser.Permissions.Add(permission);
            }
            var subFolders = Stores.Where(s => s.ParentStoreID == permission.StoreID).ToList();
            foreach (var subFolder in subFolders)
            {
                var subFolderPermission = new Permission
                {
                    ID = permission.ID + 1, // or generate a unique ID
                    DriverID = permission.DriverID,
                    StoreID = subFolder.StoreID,
                    UserID = permission.UserID,
                    Roles = permission.Roles
                };
                SharePermissions(subFolderPermission, targetUser);
            }
        }

        public Permission GetPermissionForUserInDriver(int userId, int driverId)
        {
            return Permissions.FirstOrDefault(p => p.UserID == userId && p.DriverID == driverId);
        }

        public Permission GetPermissionForUserInStored(int userId, int? storeID)
        {
            Permission permission;
            permission = Permissions.FirstOrDefault(p => p.UserID == userId && p.StoreID == storeID);
            return permission;
        }

        public string Name { get; set; }
        public int ID { get; set; }
        private List<Store> Stores { get; set; }
        public List<Permission> Permissions { get; set; }
    }

    public class UserBuilder
    {
        private readonly User _user;

        public UserBuilder()
        {
            _user = new User();
        }

        public UserBuilder WithName(string name)
        {
            _user.Name = name;
            return this;
        }

        public UserBuilder WithID(int id)
        {
            _user.ID = id;
            return this;
        }

        public UserBuilder AddStore(Store store)
        {
            _user.AddToDrive(store);
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}
