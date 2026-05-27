class Car:
    def __init__(self, mark, model, color, yearOfRelease):
        self._mark = mark
        self._model = model
        self._color = color
        self._yearOfRelease = yearOfRelease

    @property
    def mark(self):
        return self._mark
    @mark.setter
    def mark(self, value):
        if not value:
            raise ValueError()
        self._mark = value
        
    @property
    def model(self):
        return self._model    
    @model.setter
    def model(self, value):
        if not value:
            raise ValueError()
        self._model = value

    @property
    def color(self):
        return self._color    
    @color.setter
    def color(self, value):
        if not value:
            raise ValueError()
        self._color = value

    @property
    def yearOfRelease(self):
        return self._yearOfRelease    
    @yearOfRelease.setter
    def yearOfRelease(self, value):
        if not value:
            raise ValueError()
        self._yearOfRelease = value
    

car1 = Car("BMW", "M3", "red", 2021)
print(car1.mark)
car1.mark = "bmw"
print(car1.mark)

car2 = Car("Mercedes-Benz", "W214", "white", 2024)
print(car2.model)
car2.model = "w214"
print(car2.model)

car3 = Car("Xiaomi", "YU7", "red", 2025)
print(car3.color)
car3.color = "green"
print(car3.color)

car4 = Car("Chevrolet", "Camaro", "gray", 2019)
print(car4.yearOfRelease)
car4.yearOfRelease = 2023
print(car4.yearOfRelease)
