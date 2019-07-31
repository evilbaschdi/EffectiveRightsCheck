using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace EffectiveRightsCheck.Core
{
    public static class FileSystemEffectiveRights
    {
        public static FileSystemRights GetRights(string userName, string path)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("userName");
            }

            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new ArgumentException($"path: {path}");
            }

            return GetEffectiveRights(userName, path);
        }


        /// <summary>
        ///     based on the rules retrieved figure out if the user has access
        /// </summary>
        /// <param name="userName">user name no domain</param>
        /// <param name="path">file share path</param>
        /// <returns></returns>
        private static FileSystemRights GetEffectiveRights(string userName, string path)
        {
            FileSystemAccessRule[] accessRules = GetAccessRulesArray(userName, path);
            FileSystemRights denyRights = 0;
            FileSystemRights allowRights = 0;

            for (int index = 0, total = accessRules.Length; index < total; index++)
            {
                FileSystemAccessRule rule = accessRules[index];

                if (rule.AccessControlType == AccessControlType.Deny)
                {
                    denyRights |= rule.FileSystemRights;
                }
                else
                {
                    allowRights |= rule.FileSystemRights;
                }
            }

            return (allowRights | denyRights) ^ denyRights;
        }

        /// <summary>
        ///     Compare the file system access rules with the sids comming from the user
        ///     if we might have a deny rule or an allow rule
        /// </summary>
        /// <param name="userName">user name without domain</param>
        /// <param name="path">path to file</param>
        /// <returns></returns>
        private static FileSystemAccessRule[] GetAccessRulesArray(string userName, string path)
        {
            // get all access rules for the path - this works for a directory path as well as a file path
            AuthorizationRuleCollection authorizationRules = (new FileInfo(path)).GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (AuthorizationRule rule in authorizationRules)
            {
            }

            // get the user's sids
            string[] sids = GetSecurityIdentifierArray(userName);

            // get the access rules filtered by the user's sids
            return (from rule in authorizationRules.Cast<FileSystemAccessRule>()
                    where sids.Contains(rule.IdentityReference.Value)
                    select rule).ToArray();
        }

        /// <summary>
        ///     Get the group SIDS of the current user
        ///     assumption: that users are unique within the domain
        /// </summary>
        /// <param name="userName">user's name without the domain</param>
        /// <returns>array of sids</returns>
        private static string[] GetSecurityIdentifierArray(string userName)
        {
            // connect to the domain
            UserPrincipal user;
            try
            {
                using (var pc = new PrincipalContext(ContextType.Domain))
                {
                    user = new UserPrincipal(pc)
                           {
                               SamAccountName = userName
                           };
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }


            var searcher = new PrincipalSearcher
                           {
                               QueryFilter = user
                           };
            user = searcher.FindOne() as UserPrincipal;

            if (user == null)
            {
                throw new ApplicationException($"Invalid User Name:  {userName}");
            }

            // use WindowsIdentity to get the user's groups
            var windowsIdentity = new WindowsIdentity(user.UserPrincipalName);
            if (windowsIdentity.Groups != null)
            {
                var sids = new string[windowsIdentity.Groups.Count + 1];

                if (windowsIdentity.User != null)
                {
                    sids[0] = windowsIdentity.User.Value;
                }

                for (int index = 1, total = windowsIdentity.Groups.Count; index < total; index++)
                {
                    sids[index] = windowsIdentity.Groups[index].Value;
                }

                return sids;
            }

            return new string[] { };
        }
    }
}