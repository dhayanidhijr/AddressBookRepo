using System.Collections.Generic;

namespace AddressBookBusinessLib.Interface
{
    public interface IBusinessRepository<Model, ModelKey> where Model : IBusinessObject
    {
        bool Create(Model model);

        IEnumerable<Model> ReadAll();

        Model Read(ModelKey key);

        bool Update(Model model);

        bool Delete(ModelKey key);
    }
}
