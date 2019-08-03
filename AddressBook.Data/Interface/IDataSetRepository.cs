using System.Collections.Generic;

namespace AddressBookDataLib.Interface
{
    public interface IDataSetRepository<Model, ModelKey>
    {
        bool Create(Model model);

        IEnumerable<Model> ReadAll();

        Model Read(ModelKey key);

        bool Update(Model model);

        bool Delete(ModelKey key);
    }
}
