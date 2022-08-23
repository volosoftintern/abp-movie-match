using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace MovieMatch
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IdentityUserManager))]
    public class MyIdentityUserManager : IdentityUserManager
    {
        public MyIdentityUserManager(IdentityUserStore store, IIdentityRoleRepository roleRepository, IIdentityUserRepository userRepository, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<Volo.Abp.Identity.IdentityUser> passwordHasher, IEnumerable<IUserValidator<Volo.Abp.Identity.IdentityUser>> userValidators, IEnumerable<IPasswordValidator<Volo.Abp.Identity.IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<IdentityUserManager> logger, ICancellationTokenProvider cancellationTokenProvider, IOrganizationUnitRepository organizationUnitRepository, ISettingProvider settingProvider) : base(store, roleRepository, userRepository, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger, cancellationTokenProvider, organizationUnitRepository, settingProvider)
        {
        }
        public async override Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            if(user.GetProperty("Photo")==null)
            {
                user.SetProperty("Photo", "default_picture.png");

            }
            return await base.CreateAsync(user);
        }
    }
}
