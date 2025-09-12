using FluentValidation;
using System.Text.RegularExpressions;

namespace TravelDoc.Api.Infrastructure.Common.Extensions.Validators
{
    public static class CpfValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Must(IsCpfValid).WithMessage("CPF inválido.");
        }

        private static bool IsCpfValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11)
                return false;

            // Rejeita CPFs com todos os dígitos iguais
            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            var tempCpf = cpf.Substring(0, 9);
            var firstDigit = CalculateCpfDigit(tempCpf);
            var secondDigit = CalculateCpfDigit(tempCpf + firstDigit);

            return cpf.EndsWith($"{firstDigit}{secondDigit}");
        }

        private static string CalculateCpfDigit(string cpfBase)
        {
            int sum = 0;
            int multiplier = cpfBase.Length + 1;

            foreach (char digitChar in cpfBase)
            {
                sum += (digitChar - '0') * multiplier--;
            }

            int remainder = sum % 11;
            if (remainder < 2)
                return "0";

            return (11 - remainder).ToString();
        }
    }
}
