using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nebula.CoreLibrary.IOC;
using Nebula.Membership.Repositories;

namespace Nebula.Membership
{
    public interface ICompanyService
    {
        Company GetCompany(User user);
        Company CreateCompany(Company company);
        Company CreateCompany(string companyName);
        void DeleteCompany(Company company, bool softDelete = false);
        Company UpdateCompany(Company company);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public Company GetCompany(User user)
        {
            return _companyRepository.Query(v => v.Id == user.CompanyId).FirstOrDefault();
        }

        public Company CreateCompany(Company company)
        {
            if(_companyRepository.Query(v => v.Name == company.Name).Any()) throw new Exception("Name cannot be exists!");
            if (_companyRepository.Query(v => v.CommercialName == company.CommercialName).Any()) throw new Exception("Commercial Name cannot be exists!");

            _companyRepository.Create(company);
            _companyRepository.Commit();
            return company;
        }

        

        public Company CreateCompany(string companyName)
        {
            Company company = new Company {Name = companyName};
            return CreateCompany(company);
        }

        public void DeleteCompany(Company company, bool softDelete = false)
        {
            if (softDelete)
            {
                _companyRepository.Delete(company);
                _companyRepository.Commit();
            }
            else
            {
                company.IsDeleted = true;
                UpdateCompany(company);
            }
        }

        public Company UpdateCompany(Company company)
        {

            if (_companyRepository.Query(v => v.Name == company.Name && v.Id != company.Id).Any()) throw new Exception("Name cannot be exists!");
            if (_companyRepository.Query(v => v.CommercialName == company.CommercialName && v.Id != company.Id).Any()) throw new Exception("Commercial Name cannot be exists!");

            _companyRepository.Update(company);
            _companyRepository.Commit();
            return company;
        }
    }

    public static class CompanyExtensions
    {

        public static Company GetCompany(this User user)
        {
            var companyService = DependencyService.GetService<CompanyService>();
            return companyService.GetCompany(user);
        }
    }
}
