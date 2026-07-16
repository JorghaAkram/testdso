using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prove.Utilities.Constants
{
    public enum OrderBy
    {
        ASC, DESC
    }
    public static class GeneralConstant
    {

        public static bool ISPRODUCTION = true;
        public static bool IsProduction { get; set; } = false;
        /// <summary>
        /// File place Holder
        /// </summary>
        public static readonly string IMG_PLACEHOLDER = "wwwroot/img/place_holder.png";

        public static string NO = "N";
        public static string YES = "Y";
        public static string CREATE = "CREATE";
        public static string UPDATE = "UPDATE";
        public static string DELETE = "DELETE";

        public static string DATE_FORMAT_1 = "dd MM yyyy HH:mm";
        public static string DATE_FORMAT_2 = "dd MMM yyyy HH:mm";
        public static string DATE_FORMAT_3 = "dd MMMM yyyy HH:mm";

        public static string DATE_FORMAT_4 = "d MMM yyyy HH:mm";

        public static string DATE_ONLY_FORMAT_2 = "dd MMM yyyy";
        public static string DATE_PICKER_FORMAT_2 = "dd M yy";

        public static string TIME_ONLY_FORMAT_2 = "HH:mm";

        public static string EMPTY_STRING = "";
        public static string SPACE = " ";
        public static string UNDERSCORE = "_";

        public const string SUCCESS = nameof(SUCCESS);
        public const string FAILED = nameof(FAILED);


        public static string SITConnectionMode = nameof(SITConnectionMode);
        public static string DevConnectionMode = nameof(DevConnectionMode);
        public static string ProdConnectionMode = nameof(ProdConnectionMode);

        /* session */

        public const string JWT = "_jwt";
        public const string ENCRYPT_PASSWORD = "##extra29";
        public const string ASP_USER_ID = "_aspUserId";
        public const string USER_ID = "_userId";
        public const string USERNAME = "_userName";
        public const string NAME = "_name";
        public const string EMAIL = "_email";
        public const string USERTYPE = "_userType";


        /* RISK CATEGORY */
        public const string HIGH_RISK = "HIGH RISK";
        public const string MIDDLE_RISK = "MIDDLE RISK";
        public const string LOW_RISK = "LOW RISK";
        public const string UNDER_STANDARD = "UNDER STANDARD";

        /* HSSE OBIT CARD */
        public const int HSSE_FOLLOWUP_CLEAREXECUTION = 4;
        public const int HSSE_FOLLOWUP_NOTFOLLOWEDUP = 3;
        public const int HSSE_FOLLOWUP_BEINGFOLLOWEDUP = 2;
        public const int HSSE_FOLLOWUP_HASBEENFOLLOWEDUP = 1;

        public static string DayOrDateToString(this DateTime? dateTime, bool isDate = true)
            => dateTime.HasValue ? dateTime.Value.ToString(isDate ? "d MMM yyyy" : "HH:mm") : string.Empty;


        /* CSMS STATUS */
        public const string UNANSWERED = nameof(UNANSWERED);
        public const string PENDINGUSER = nameof(PENDINGUSER);
        public const string PENDINGPROC = nameof(PENDINGPROC);
        public const string PENDINGHSSE = nameof(PENDINGHSSE);
        public const string USER_VERIFICATION = nameof(USER_VERIFICATION);
        public const string BUILDCERT = nameof(BUILDCERT);
        public const string WAITING_SIGNATURE = nameof(WAITING_SIGNATURE);
        public const string REVISE = nameof(REVISE);

        /* Upload */
        public const string URL_CREATE_UPLOAD_NEW = "D:/Sharing/";
        public const string URL_VIEW_UPLOAD_NEW = "/upload/";
        public const string URL_DELETE_UPLOAD_NEW = "D:/Sharing/PrideApp/wwwroot";
        public const string URL_DOWNLOAD_UPLOAD_NEW = "D:/Sharing";
        public const string DELETED_PATH = "/upload/deleted/";


        /* Presence */
        public const int MORNING_START = 5;
        public const int MORNING_END = 10;
        public const int AFTERNOON_START = 15;
        public const int AFTERNOON_END = 19;
        public const int PRESENCE_12 = 12;
        public const string PRESENCE_MORNING = "PAGI";
        public const string PRESENCE_AFTERNOON = "SORE";

        public const string FLAG_FILE_IVENDZ_INVOICE = "IVENDZ_INVOICE";
        public const string FLAG_FILE_IVENDZ_OFFSETTING = "IVENDZ_OFFSETTING";
        public const string FLAG_FILE_IVENDZ_PPH_EVIDENCE = "IVENDZ_PPH_EVIDENCE";

        public const string FLAG_PROC_BAST = "PROC_BAST";
        public const string FLAG_PROC_BA_NEGOSIASI = "PROC_BA_NEGOSIASI";
        public const string FLAG_FILE_PROC_VENDOR_EVIDENCE = "PROC_VENDOR_EVIDENCE";
        public const string FLAG_FILE_PROC_VENDOR_ITEMS = "PROC_VENDOR_ITEMS";
        public const string FLAG_PROC_VENDOR_APPROVAL = "VENDOR_APPROVAL";

        /* MOMO */
        public const string FLAG_PORT_INFORMATION = "PORT_INFORMATION";
        public const string FLAG_REGULATION_REQUIREMENT = "REGULATION_REQUIREMENT";





        public static string CreateUploadPathNew(string path)
        {
            return Path.Combine(URL_CREATE_UPLOAD_NEW, Path.Combine("upload/", Path.Combine(path, DateTime.UtcNow.ToString("yyyyMMdd/"))));
        }

        public static string ReplaceDeletedPath(string path)
        {
            return path.Replace(URL_VIEW_UPLOAD_NEW, DELETED_PATH);
        }

        public static string CreateDeletedPath(string path)
        {
            return Path.Combine(URL_CREATE_UPLOAD_NEW, path);
        }


        public static string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            System.Random random = new System.Random();
            for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
            {
                password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                bool moreThanTwoIdenticalInARow =
                    characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                    && password[characterPosition] == password[characterPosition - 1]
                    && password[characterPosition - 1] == password[characterPosition - 2];

                if (moreThanTwoIdenticalInARow)
                {
                    characterPosition--;
                }
            }

            return string.Join(null, password);
        }


        public static string CreateUploadPathViewNew(string path) => Path.Combine(Path.Combine(URL_VIEW_UPLOAD_NEW, path), DateTime.UtcNow.ToString("yyyyMMdd/"));

        /* File Flag */
        public const string FLAG_FILE_PARTNER_ADMINISTRATIONPHASE1 = "PARTNER_ADMINISTRATIONPHASE1";


        #region PROVE
        public const string PROVE_REQUEST = "Request";
        public const string PROVE_REVIEW = "Review";
        public const string PROVE_REVISED = "Revised";
        public const string PROVE_NEED_SIGN = "Need Sign";
        public const string PROVE_SIGNED = "Signed";
        public const string PROVE_REVIEWED = "Reviewed";
        public const string PROVE_UPLOADED = "Uploaded";
        public const string PROVE_DELETED = "Deleted";
        #endregion

    }
}
