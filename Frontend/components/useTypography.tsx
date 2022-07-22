import { Text } from "@chakra-ui/react";

//@ts-ignore
export const PassText = ({ children }) => {
  return (
    <Text fontWeight="bold" fontSize="4xl">
      {children}
    </Text>
  );
};
