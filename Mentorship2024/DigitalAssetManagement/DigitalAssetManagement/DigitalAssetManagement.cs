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
        public void TestNumberOfStoresInDrive()
        {
            // Arrange
            var user = new UserBuilder()
                .WithName("John")
                .WithID(1)
                .Build();

            var googleDriver = new Driver { ID = 1, DriveName = "GoogleDriver" };
            var oneDriver = new Driver { ID = 2, DriveName = "GoogleDriver" };
            user.AddDriverToUser(googleDriver);
            user.AddDriverToUser(oneDriver);


            // Act
            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.File, StoreName = "summary1.txt", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "summary2.txt", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 3, StoreType = StoreType.File, StoreName = "summary3.txt", ParentDriverID = 2, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 4, StoreType = StoreType.File, StoreName = "summary4.txt", ParentDriverID = 2, ParentStoreID = null });

            // Assert
            var googleDriveStores = user.GetStores().Where(f => f.ParentDriverID == 1).ToList();
            var oneDriveStores = user.GetStores().Where(f => f.ParentDriverID == 2).ToList();

            Assert.Equal(2, googleDriveStores.Count);
            Assert.Equal(2, oneDriveStores.Count);
        }

        [Fact]
        public void TestInitialFile()
        {
            // Arrange
            var user = new UserBuilder()
                .WithName("John")
                .WithID(1)
                .Build();

            var driver = new Driver { ID = 1, DriveName = "GoogleDrive" };
            user.AddDriverToUser(driver);

            // Act
            user.AddToDrive(new Store { StoreID = 1, StoreType = StoreType.File, StoreName = "summary1.txt", ParentDriverID = 1, ParentStoreID = null });
            user.AddToDrive(new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "summary2.txt", ParentDriverID = 1, ParentStoreID = null });

            // Assert
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

            // share permission
            var adminPermission = new Permission { ID = 1, DriverID = 1, StoreID = 1, UserID = 1, Roles = Role.Admin };
            adminUser.Permissions.Add(adminPermission); // Admin user gives themselves admin permission

            var contributorPermission = new Permission { ID = 2, DriverID = 1, StoreID = 2, UserID = 2, Roles = Role.Contributor };
            adminUser.SharePermissions(contributorPermission, contributorUser);

            adminUser.AddToDrive(rootFolder);
            adminUser.AddToDrive(subFolder);

            // Assert
            var sharedPermission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, subFolder);

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

            var driver = new Driver { ID = 1, DriveName = "GoogleDrive" };
            adminUser.AddDriverToUser(driver);

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
            var sharedPermission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, rootFolder);

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

            var driver = new Driver { ID = 1, DriveName = "GoogleDrive" };
            adminUser.AddDriverToUser(driver);

            var rootFolder = new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Root1", ParentDriverID = 1, ParentStoreID = null };
            var subFolder1 = new Store { StoreID = 2, StoreType = StoreType.Folder, StoreName = "SubFolder1", ParentDriverID = 1, ParentStoreID = 1 };
            var subFolder2 = new Store { StoreID = 3, StoreType = StoreType.Folder, StoreName = "SubFolder2", ParentDriverID = 1, ParentStoreID = 2 };

            // Add folders before assigning permissions
            adminUser.AddToDrive(rootFolder);
            adminUser.AddToDrive(subFolder1);
            adminUser.AddToDrive(subFolder2);

            var adminPermission = new Permission { ID = 1, DriverID = 1, StoreID = 1, UserID = 1, Roles = Role.Admin };
            adminUser.SharePermissions(adminPermission, adminUser); // Admin user gives themselves admin permission

            var contributorPermission = new Permission { ID = 2, DriverID = 1, StoreID = 1, UserID = 2, Roles = Role.Contributor };
            adminUser.SharePermissions(contributorPermission, contributorUser);

            // Assert

            // Check SubFolder2
            var subFolder2Permission = contributorUser.GetPermissionForUserInStored(contributorUser.ID, subFolder2);
            Assert.NotNull(subFolder2Permission);
            Assert.Equal(Role.Contributor, subFolder2Permission?.Roles);
            Assert.True(subFolder2Permission?.CanAdd());
            Assert.True(subFolder2Permission?.CanModify());
            Assert.True(subFolder2Permission?.CanDelete());
        }

        [Fact]
        public void ContributorAddingAndModifyingFiles()
        {
            // Arrange
            var contributorUser = new UserBuilder()
                .WithName("contributor1")
                .WithID(2)
                .Build();

            var readerUser = new UserBuilder()
                .WithName("reader1")
                .WithID(3)
                .Build();

            var driver = new Driver { ID = 1, DriveName = "GoogleDrive" };
            contributorUser.AddDriverToUser(driver);

            var specificationsFolder = new Store { StoreID = 1, StoreType = StoreType.Folder, StoreName = "Specifications", ParentDriverID = 1, ParentStoreID = null };

            var spec1 = new Store { StoreID = 2, StoreType = StoreType.File, StoreName = "Spec1.pdf", ParentDriverID = 1, ParentStoreID = 1 };

            contributorUser.AddToDrive(specificationsFolder);
            contributorUser.AddToDrive(spec1);


            var readerPermission = new Permission { ID = 2, DriverID = 1, StoreID = 1, UserID = 3, Roles = Role.Reader };
            readerUser.Permissions.Add(readerPermission);

            var readerStore2Permission = new Permission { ID = 3, DriverID = 1, StoreID = 2, UserID = 3, Roles = Role.Reader };
            readerUser.Permissions.Add(readerStore2Permission);

            readerUser.AddStore(specificationsFolder);
            readerUser.AddStore(spec1);
            // Act and Assert for Contributor
            // 1. Add Spec3.pdf
            var spec3 = new Store { StoreID = 4, StoreType = StoreType.File, StoreName = "Spec3.pdf", ParentDriverID = 1, ParentStoreID = 1 };
            contributorUser.AddToDrive(spec3);

            // 2. Rename Spec1.pdf to Spec1_Final.pdf
            contributorUser.RenameFile(2, "Spec1_Final.pdf");

            // Assert for Contributor
            var spec3File = contributorUser.GetStores().FirstOrDefault(s => s.StoreID == 4);
            Assert.NotNull(spec3File);
            Assert.Equal("Spec3.pdf", spec3File?.StoreName);

            var renamedSpec1 = contributorUser.GetStores().FirstOrDefault(s => s.StoreID == 2);
            Assert.NotNull(renamedSpec1);
            Assert.Equal("Spec1_Final.pdf", renamedSpec1?.StoreName);

            // Act and Assert for Reader (should throw exceptions)
            // Attempt to add Spec4.pdf
            var spec4 = new Store { StoreID = 5, StoreType = StoreType.File, StoreName = "Spec4.pdf", ParentDriverID = 1, ParentStoreID = 1 };
            Assert.Throws<UnauthorizedAccessException>(() => readerUser.AddToDrive(spec4));

            // Attempt to rename Spec1_Final.pdf to Spec1_Rev1.pdf
            Assert.Throws<UnauthorizedAccessException>(() => readerUser.RenameFile(2, "Spec1_Rev1.pdf"));
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
        public int ? OwnerID { get; set; }
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
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store), "Store cannot be null.");
            }
            var permission = GetPermissionForUserInStored(ID, store);
            if (permission?.CanAdd() ?? false)
            {
                Stores.Add(store);
                var newStorePermission = new Permission
                {
                    ID = store.StoreID, // Assign a unique ID for the permission
                    DriverID = store.ParentDriverID, // Same driver as the parent
                    StoreID = store.StoreID, // Permission for the specific store
                    UserID = ID, // The current user
                    Roles = permission.Roles // Same role as the parent permission
                };
                Permissions.Add(newStorePermission);
            }
            else
            {
                throw new UnauthorizedAccessException("User does not have permission to add files.");
            }
        }
        public void AddDriverToUser(Driver driver)
        {
            User user = this;
            driver.OwnerID = user.ID;

            // Automatically grant Admin role for the user who is now the owner of the driver
            var permission = new Permission
            {
                ID = driver.ID, 
                DriverID = driver.ID,
                UserID = user.ID,
                Roles = Role.Admin 
            };

            user.Permissions.Add(permission); 
        }
        public void AddStore(Store store)
        {
            if (!Stores.Contains(store))
            {
                Stores.Add(store);
            }
        }

        public void RenameFile(int storeId, string newFileName)
        {
            var lstStore = GetStores();
            var store = lstStore.FirstOrDefault(s => s.StoreID == storeId);
            if (store == null)
            {
                throw new NullReferenceException("Store does not exist.");
            }

            var permission = GetPermissionForUserInStored(ID, store); // Ensure permission is checked
            if (permission?.CanModify() ?? false)
            {
                store.StoreName = newFileName;
            }
            else
            {
                throw new UnauthorizedAccessException("User does not have permission to rename files.");
            }
        }

        public List<Store> GetStores()
        {
            // Load stores based on the user's permissions
            var accessibleStores = new List<Store>();

            foreach (var permission in Permissions)
            {
                // Retrieve stores based on the StoreID and DriverID from permissions
                var store = Stores.FirstOrDefault(s => s.StoreID == permission.StoreID);
                if (store != null && permission.CanView()) // Check if user has view permissions
                {
                    accessibleStores.Add(store);
                }
            }

            return accessibleStores;
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

        public Permission GetPermissionForUserInStored(int userId, Store store)
        {
            var permission = Permissions.FirstOrDefault(p => p.UserID == userId && p.StoreID == store.StoreID);

            if (permission == null)
            {
                // Check for permission at the DriverID level (parent level)
                permission = Permissions.FirstOrDefault(p => p.UserID == userId && p.DriverID == store.ParentDriverID);
            }

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
