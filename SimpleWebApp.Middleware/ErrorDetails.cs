using Newtonsoft.Json;
using System.Text;

namespace SimpleWebApp.Middleware
{
    public class ErrorDetails
    {
        private IEnumerable<ValidationFailure> _errors;

        public ErrorDetails(IEnumerable<ValidationFailure> errors)
        {
            _errors = errors ?? Enumerable.Empty<ValidationFailure>();
        }

        public string SerializeToJson()
        {
            var result = ToCommonErrorModel();
            return JsonConvert.SerializeObject(result);
        }

        public Dictionary<string, LinkedList<string>> ToCommonErrorModel()
        {
            var groupedByProp = _errors.GroupBy(x =>
            {
                var key = MakeDottedIdentifierLowercase(x.PropertyName);

                if (key == string.Empty)
                {
                    return "@";
                }

                return key;
            });

            var errorResult = new Dictionary<string, LinkedList<string>>();

            foreach (IGrouping<string, ValidationFailure> validationFailures in groupedByProp)
            {
                errorResult.Add(validationFailures.Key, new LinkedList<string>());

                foreach (var validationFailure in validationFailures)
                {
                    errorResult[validationFailures.Key].AddLast(validationFailure.ErrorMessage);
                }
            }

            return errorResult;
        }

        private string MakeDottedIdentifierLowercase(string key)
        {
            if (key.Length == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(key.Length);

            sb.Append(char.ToLowerInvariant(key[0]));

            for (int i = 1; i < key.Length; i++)
            {
                sb.Append(key[i]);

                if (key[i] == '.')
                {
                    i++;
                    sb.Append(char.ToLowerInvariant(key[i]));
                }
            }

            return sb.ToString();
        }
    }
}
