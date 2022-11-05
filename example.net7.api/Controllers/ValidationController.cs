using FluentValidation;
using Hoyo.Extension.Validations;
using Microsoft.AspNetCore.Mvc;

namespace example.net7.api.Controllers
{
    /// <summary>
    /// 验证测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {

        private readonly IValidator<Test> _validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        public ValidationController(IValidator<Test> validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// 测试验证
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> Post(Test test)
        {

            var validator = await _validator.ValidateAsync(test);

            if (!validator.IsValid)
            {
                var validation = new ValidationException(validator.Errors);
                return BadRequest(validation.ToProblemDetails());
            }
            return Ok();
        }


    }






}



public class Test
{

    public int Id { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }
}

public class TestValidator : AbstractValidator<Test>
{
    public TestValidator()
    {

        this.RuleFor(o => o.Name).NotEmpty();
        this.RuleFor(o => o.Title).NotEmpty();
    }

}

