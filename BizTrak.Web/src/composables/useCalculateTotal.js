export function useCalculateTotal(tasks, materials) {
    const taskTotal = tasks
      .filter(task => !task.isToBeDeleted)
      .reduce((sum, task) => sum + parseFloat(task.cost || 0), 0);
  
    const materialTotal = materials
      .filter(material => !material.isToBeDeleted)
      .reduce((sum, material) => sum + (parseFloat(material.quantity || 0) * parseFloat(material.unit_price || 0)), 0);
  
    return (taskTotal + materialTotal).toFixed(2);
  }