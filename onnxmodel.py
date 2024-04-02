import onnx # not used but can be used to inspect and manipulate saved onnx model
from skl2onnx import convert_sklearn
from skl2onnx.common.data_types import FloatTensorType

from sklearn.tree import DecisionTreeClassifier
from sklearn.model_selection import train_test_split
import pandas as pd

df_classes = pd.read_csv('class.csv')

df = pd.read_csv('zoo.csv')

for col in df:
  if not pd.api.types.is_numeric_dtype(df[col]) and col != 'class_type' and col != 'animal_name':
    df = pd.get_dummies(df, columns=[col], drop_first=True)

y = df.class_type
X = df.drop(columns=['class_type','animal_name'])

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=.3, random_state=4) # state = 4 for reptile example

model = DecisionTreeClassifier(max_depth=5).fit(X_train, y_train)

print(f'Accuracy:\t{model.score(X_test, y_test)}')
print(y_test.value_counts() / y_test.shape[0])

# # TESTING
# # Example input features
# hair, feathers, eggs, milk, airborne, aquatic, predator, toothed, backbone, breathes, venomous, fins, legs, tail, domestic, catsize
# input_features = [0,1,1,0,1,0,0,0,0,1,0,0,2,0,0,0] 
# print(model.predict(input_features))


# Converting the model to ONNX format
initial_type = [('float_input', FloatTensorType([None, X_train.shape[1]]))]
onnx_model = convert_sklearn(model, initial_types=initial_type)

# Saving the model
with open("decision_tree_model.onnx", "wb") as f:
    f.write(onnx_model.SerializeToString())


