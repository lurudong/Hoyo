using FluentValidation;

namespace Hoyo.Extension.Validations
{
    /// <summary>
    /// 验证扩展
    /// </summary>
    public static class ValidationExtensions
    {

        /// <summary>
        /// 验证错误标准
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ValidationProblemDetails ToProblemDetails(this ValidationException ex)
        {
            var error = new ValidationProblemDetails
            {

                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
                Status = 400,  //自定义成可以,
                Title = "发生了一个或多个验证错误",
                Errors = new Dictionary<string, string[]>()
            };

            //返回的错误格式自行修改，不建议Errors 用字典，页面显示麻烦

            foreach (var validationFailure in ex.Errors)
            {

                var erros = error.Errors;

                if (erros.ContainsKey(validationFailure.PropertyName))
                {

                    erros[validationFailure.PropertyName] = erros[validationFailure.PropertyName].Concat(new[] { validationFailure.ErrorMessage }).ToArray();
                    continue;
                }

                error.Errors.Add(validationFailure.PropertyName, new[] { validationFailure.ErrorMessage });
            }

            return error;
        }

    }


    /// <summary>
    /// 标准类
    /// </summary>
    public class ValidationProblemDetails : ProblemDetails
    {

    }
}
